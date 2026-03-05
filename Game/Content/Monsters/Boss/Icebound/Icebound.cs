using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class Icebound : SavvasIceStorm, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * CharacterCount,
				Traits = (stats.Traits ?? [])
				.Append(new ConditionImmunityTrait(Conditions.Stun))
				.Append(new ConditionImmunityTrait(Conditions.Disarm))
				.Append(ConditionImmunityTrait.PoisonImmunityTrait())
				.Append(new ConditionImmunityTrait(Conditions.Immobilize))
				.Append(new ForcedMovementImmunityTrait())
				.ToArray()
			})
			.ToArray();

	public override string Name => "Icebound";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<SavvasIceStorm>();

	private bool _summonElite;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	public string GetSpecial1Description(Monster monster) => $"""
	                                                          If the Icebound is occupying Room 1 (A2b tile), summon one Wind Demon in an empty hex closest to the hex marked {Icons.InlineMarker(Marker.Type.c)}. If the Icebound is occupying Room 2 (A3a tile), summon one Frost Demon in an empty hex closest to the hex marked {Icons.InlineMarker(Marker.Type.a)}. If the Icebound is occupying the G2a tile, summon one Stone Golem in an empty hex closest to the hex marked {Icons.InlineMarker(Marker.Type.b)}. Summons are normal for two characters, every other summon is elite for three characters, and all summons are elite for four characters.
	                                                          ”{Icons.Inline(Icons.Heal)}3, Self”.
	                                                          """;

	public string GetSpecial2Description(Monster monster) => $"""
	                                                          {Icons.Inline(Icons.Attack)}+0, {Icons.Inline(Icons.Targets)} all, {Icons.Inline(Icons.Range)}3.
	                                                          If the Icebound is occupying Room 1 (A2b tile), it immediately jumps into the nearest unoccupied hex adjacent to {Icons.InlineMarker(Marker.Type.e)}. If the Icebound is occupying Room 2 (A3a tile), it immediately jumps into the nearest unoccupied hex adjacent to {Icons.InlineMarker(Marker.Type.d)}.
	                                                          {Icons.Inline(Icons.Attack)}+0, {Icons.Inline(Icons.Targets)} all, {Icons.Inline(Icons.Range)}3
	                                                          """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterSummonAbility.Builder()
			.WithMonsterModel(CalculateMonsterModel(monster))
			.WithMonsterType(CharacterCount >= 4 || (CharacterCount == 3 && _summonElite) ? MonsterType.Elite : MonsterType.Normal)
			.WithGetValidHexes((state, list) =>
			{
				Hex spawnHex = CalculateSpawnPoint(monster);
				List<Hex> hexes = RangeHelper.GetHexesInRange(spawnHex, 100, requiresLineOfSight: false).ToList();
				hexes.Shuffle(GameController.Instance.StateRNG);
				hexes.Sort((otherHexA,
					otherHexB) => RangeHelper.Distance(spawnHex,
						otherHexA)
					.CompareTo(RangeHelper.Distance(spawnHex,
						otherHexB)));
				Hex firstHex = hexes.FirstOrDefault(hex => hex.IsEmpty());

				if(firstHex == null)
				{
					return;
				}

				int distance = RangeHelper.Distance(spawnHex,
					firstHex);

				list.AddRange(
					hexes.Where(h => h.IsEmpty() &&
					                 RangeHelper.Distance(spawnHex, h) == distance)
				);
			})
			.WithOnAbilityEndedPerformed(async state =>
			{
				_summonElite = !_summonElite;
				await GDTask.CompletedTask;
			})
		),
		new MonsterAbilityCardAbility(HealAbility.Builder()
			.WithHealValue(3)
			.WithTarget(Target.Self)
		)
	];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0).WithRange(3).WithTarget(Target.TargetAll | Target.Enemies)),
		//TODO: Teleport ability
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0).WithRange(3).WithTarget(Target.TargetAll | Target.Enemies))
	];

	private MonsterModel CalculateMonsterModel(Monster monster)
	{
		if(monster.Hex.GetRoom() == GameController.Instance.Map.Rooms[1])
		{
			return ModelDB.Monster<WindDemon>();
		}

		if(monster.Hex.GetRoom() == GameController.Instance.Map.Rooms[2])
		{
			return ModelDB.Monster<FrostDemon>();
		}

		return ModelDB.Monster<StoneGolem>();
	}

	private Hex CalculateSpawnPoint(Monster monster)
	{
		Marker marker;
		if(monster.Hex.GetRoom() == GameController.Instance.Map.Rooms[1])
		{
			marker = GameController.Instance.Map.GetMarker(Marker.Type.c);
		}
		else if(monster.Hex.GetRoom() == GameController.Instance.Map.Rooms[2])
		{
			marker = GameController.Instance.Map.GetMarker(Marker.Type.a);
		}
		else
		{
			marker = GameController.Instance.Map.GetMarker(Marker.Type.b);
		}

		return marker.Hex;
	}
}
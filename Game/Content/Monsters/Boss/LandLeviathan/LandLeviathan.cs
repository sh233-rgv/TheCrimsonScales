using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class LandLeviathan : DeepTerror, IBossMonsterModel
{
	public override MonsterStats[] NamedLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * (CharacterCount * 2 - 1),
				Traits = (stats.Traits ?? [])
					.Append(ConditionImmunityTrait.PoisonImmunityTrait())
					.Append(new ConditionImmunityTrait(Conditions.Stun))
					.Append(new ConditionImmunityTrait(Conditions.Disarm))
					.Append(new ForcedMovementImmunityTrait())
				.ToArray()
			})
			.ToArray();

	public override string Name => "Land Leviathan";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<DeepTerror>();

	private bool _summonBlackImp = true;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	// IBossMonsterModel
	public string GetSpecial1Description(Monster monster) => $"""
	                                                          {Icons.Inline(Icons.Attack)}{monster.Stats.Attack - 1}, {Icons.Inline(Icons.Range)}5, {Icons.Inline(Icons.Targets)}2.
	                                                          Increase the Land Leviathan's maximum hit point value by 2.
	                                                          {Icons.Inline(Icons.Heal)}2, self.
	                                                          """;

	public string GetSpecial2Description(Monster monster) => $"""
	                                                          Summon one Imp in the closest empty hex within {Icons.Inline(Icons.Range)}2.
	                                                          Grant all Imps within {Icons.Inline(Icons.Range)}5:
	                                                          {Icons.Inline(Icons.Heal)}1, self
	                                                          The type of Imp that is summoned cycles in the order of Black Imp, then Forest Imp. All summons are normal for two characters. Black Imp summons are elite for three characters. All summons are elite for four characters.
	                                                          """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -1).WithTargets(2).WithRange(5)),

		new MonsterAbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				monster.SetMaxHealth(monster.MaxHealth + 2);
				await GDTask.CompletedTask;
			})
		),

		new MonsterAbilityCardAbility(HealAbility.Builder()
			.WithHealValue(2)
			.WithTarget(Target.Self)
		)
	];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterSummonAbility.Builder()
			.WithMonsterModel(_summonBlackImp ? ModelDB.Monster<BlackImp>() : ModelDB.Monster<ForestImp>())
			.WithMonsterType(_summonBlackImp
				? ((CharacterCount >= 3) ? MonsterType.Elite : MonsterType.Normal)
				: ((CharacterCount >= 4) ? MonsterType.Elite : MonsterType.Normal))
			.WithGetValidHexes((state, hexes) =>
			{
				hexes = RangeHelper.GetHexesInRange(state.Performer.Hex, 1, true).Where(hex => hex.IsEmpty()).ToList();
				if(hexes.Count == 0)
				{
					hexes = RangeHelper.GetHexesInRange(state.Performer.Hex, 2, true).Where(hex => hex.IsEmpty()).ToList();
				}
			})
			.WithOnAbilityEndedPerformed(async state =>
			{
				_summonBlackImp = !_summonBlackImp;
				await GDTask.CompletedTask;
			})
		),

		new MonsterAbilityCardAbility(GrantAbility.Builder()
			.WithGetAbilities(state =>
			[
				HealAbility.Builder().WithHealValue(1).WithTarget(Target.Self)
			])
			.WithTarget(Target.TargetAll | Target.Allies)
			.WithRange(5)
			.WithCustomGetTargets((state, targets) =>
			{
				targets.AddRange(RangeHelper.GetFiguresInRange(state.Performer, 100)
					.Where(figure => figure is Monster monster && monster.MonsterModel is Imp));
			})
		)
	];
}
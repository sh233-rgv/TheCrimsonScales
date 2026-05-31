using System;
using System.Collections.Generic;
using System.Linq;

public class EternalDemon : EarthDemon, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * CharacterCount,
				Move = stats.Move + 2,
				Traits = (stats.Traits ?? [])
				.Append(new AllNegativeConditionImmunityTrait())
				.ToArray()
			})
			.ToArray();

	public override string Name => "Eternal Demon";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<EarthDemon>();

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	public string GetSpecial1Description(Monster monster, RichTextParameters richTextParameters) =>
		$"""
		 {Icons.Inline(Icons.Teleport, richTextParameters)} to a map tile occupied by the least amount of characters.
		 {Icons.Inline(Icons.Heal, richTextParameters)} X, Self, where X is equal to the number of characters on different map tiles not occupied by the Eternal Demon.
		 """;

	public string GetSpecial2Description(Monster monster, RichTextParameters richTextParameters) =>
		$"""
		 {Icons.Inline(Icons.Attack, richTextParameters)}{monster.Stats.Attack + 1}, {Icons.Inline(Icons.Targets, richTextParameters)} all enemies sharing the same map tile.
		 """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(TeleportAbility.Builder()
			.WithCustomGetHexes((state, list) =>
				{
					list.AddRange(Scenario031.GetLeastOccupiedHexes());
				}
			)
			.Build()),
		new MonsterAbilityCardAbility(HealAbility.Builder()
			.WithHealValue(new DynamicInt<HealAbility.State>(state =>
				{
					int value = 0;
					foreach(Character character in GameController.Instance.CharacterManager.Characters)
					{
						if(character.Hex.MapTile != state.Performer.Hex.MapTile)
						{
							value++;
						}
					}

					return value;
				}
			))
			.WithTarget(Target.Self)
			.Build())
	];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, extraDamage: +1,
			customGetTargets: (state, list) =>
			{
				foreach(Figure figure in GameController.Instance.Map.Figures)
				{
					if(figure.Hex.MapTile == state.Performer.Hex.MapTile)
					{
						list.Add(figure);
					}
				}
			},
			target: Target.Enemies | Target.TargetAll)
		)
	];
}
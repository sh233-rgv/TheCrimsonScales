using System.Collections.Generic;
using System.Linq;

public class GoremyonShatterMind : MonsterModel, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 7 * CharacterCount,
			Attack = 1,
			Range = 2,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 2
		},
		new MonsterStats()
		{
			Health = 8 * CharacterCount,
			Attack = 2,
			Range = 2,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 9 * CharacterCount,
			Attack = 2,
			Range = 2,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 11 * CharacterCount,
			Attack = 2,
			Range = 3,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 4
		},
		new MonsterStats()
		{
			Health = 14 * CharacterCount,
			Attack = 3,
			Range = 3,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 4
		},
		new MonsterStats()
		{
			Health = 16 * CharacterCount,
			Attack = 3,
			Range = 3,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 5
		},
		new MonsterStats()
		{
			Health = 18 * CharacterCount,
			Attack = 3,
			Range = 4,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 5
		},
		new MonsterStats()
		{
			Health = 21 * CharacterCount,
			Attack = 4,
			Range = 4,
			Traits =
			[
				ConditionImmunityTrait.WoundImmunityTrait(), ConditionImmunityTrait.PoisonImmunityTrait(),
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ForcedMovementImmunityTrait()
			],
			CustomValue = 6
		},
	];

	public override string Name => "Goremyon Shatter-Mind";

	public override string AssetPath => "res://Content/Monsters/Boss/GoremyonShatterMind";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/MapIcon.tres";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	// IBossMonsterModel
	public string GetSpecial1Description(Monster monster) => $"""
	                                                          Cranium Overload -
	                                                          Grant Goremyon's normal ally with the fewest current hit points:
	                                                          {Icons.Inline(Icons.Move)}+1
	                                                          {Icons.Inline(Icons.Attack)}+0, {Icons.Inline(Icons.Targets)}all adjacent enemies.
	                                                          Then, the target of the grant ability is killed.
	                                                          """;

	public string GetSpecial2Description(Monster monster) => $"""
	                                                          Hostile Takeover -
	                                                          Control the character with the slowest initiative:
	                                                          {Icons.Inline(Icons.Move)}3
	                                                          {Icons.Inline(Icons.Attack)}{monster.Stats.CustomValue}
	                                                          """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(GrantAbility.Builder()
			.WithGetAbilities(state =>
			[
				MonsterAbilityCardModel.MoveAbility((Monster)state.Target, +1),
				MonsterAbilityCardModel.AttackAbility((Monster)state.Target, +0).WithTarget(Target.TargetAll | Target.Enemies).WithRange(1)
					.WithRangeType(RangeType.Melee)
			])
			.WithCustomGetTargets((state, figures) =>
			{
				List<Figure> normals = GameController.Instance.Map.Figures.Where(figure =>
					figure.AlliedWith(state.Performer) && figure is Monster monsterAlly && monsterAlly.MonsterType is MonsterType.Normal).ToList();
				if(normals.Any())
				{
					int minHealth = normals.Min(monsterAlly => monsterAlly.Health);

					figures.AddRange(normals.Where(m => m.Health == minHealth).ToList());
				}
			})),
		new MonsterAbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				await AbilityCmd.KillOrExhaust(state.ActionState.GetAbilityState<GrantAbility.State>(0).Target, state.Performer);
			})
			.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0)))
	];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(ControlAbility.Builder()
			.WithAbilities(
			[
				MoveAbility.Builder().WithDistance(3),
				AttackAbility.Builder().WithDamage(monster.Stats.CustomValue)
			])
			.WithCustomGetTargets((_, figures) =>
			{
				figures.Add(GameController.Instance.CharacterManager.Characters.Where(character => !character.IsDead)
					.MaxBy(character => character.Initiative.SortingInitiative));
			}))
	];
}
using System.Collections.Generic;
using System.Linq;

public class TerrorscaleDrake : MonsterModel, IBossMonsterModel
{
	public override MonsterStats[] BossLevelStats =>
	[
		new MonsterStats()
		{
			Health = 9 * CharacterCount,
			Move = 4,
			Attack = 3,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 10 * CharacterCount,
			Move = 4,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 13 * CharacterCount,
			Move = 5,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 1
		},
		new MonsterStats()
		{
			Health = 14 * CharacterCount,
			Move = 5,
			Attack = 4,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 2
		},
		new MonsterStats()
		{
			Health = 18 * CharacterCount,
			Move = 5,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 2
		},
		new MonsterStats()
		{
			Health = 20 * CharacterCount,
			Move = 6,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 25 * CharacterCount,
			Move = 6,
			Attack = 5,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 3
		},
		new MonsterStats()
		{
			Health = 27 * CharacterCount,
			Move = 6,
			Attack = 6,
			Traits =
			[
				new ConditionImmunityTrait(Conditions.Disarm), new ConditionImmunityTrait(Conditions.Curse),
				new ConditionImmunityTrait(Conditions.Stun), new ConditionImmunityTrait(Conditions.Immobilize),
				ConditionImmunityTrait.PoisonImmunityTrait()
			],
			CustomValue = 4
		},
	];

	public override string Name => "Terrorscale Drake";

	public override string AssetPath => "res://Content/Monsters/Boss/TerrorscaleDrake";
	public override string PortraitTexturePath => $"{AssetPath}/Portrait.tres";
	public override string MapIconTexturePath => $"{AssetPath}/MapIcon.tres";

	public override int MaxStandeeCount => 1;

	public override IEnumerable<MonsterAbilityCardModel> Deck => BossAbilityCard.Deck;

	public bool FirstForm = true;

	// IBossMonsterModel
	public string GetSpecial1Description(Monster monster) => FirstForm
		? $"""
		   Claws like Spears -
		   {Icons.Inline(Icons.Attack)}{monster.Stats.Attack + 1}, {Icons.Inline(Icons.Pierce)}3
		   All enemies adjacent to the target suffer {Icons.Inline(Icons.Damage)}1.
		   Destroy the occupied obstacle.
		   """
		: $"""
		   {Icons.Inline(Icons.Move)}{monster.Stats.Move + 1}, {Icons.Inline(Icons.Jump)}
		   {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, only {Icons.Inline(Icons.Range)}4/5, {Icons.Inline(Icons.GetCondition(Conditions.Poison1))}
		   """;

	public string GetSpecial2Description(Monster monster) => FirstForm
		? $"""
		   {Icons.Inline(Icons.Attack)}{monster.Stats.Attack - 2}
		   {Icons.Inline(Icons.Shield)}{monster.Stats.CustomValue}
		   {Icons.Inline(Icons.Retaliate)}{monster.Stats.CustomValue}
		   {Icons.Inline(Icons.Heal)}{monster.Stats.CustomValue}
		   Destroy the occupied obstacle.
		   """
		: $"""
		   {Icons.Inline(Icons.Move)}{monster.Stats.Move - 2}, {Icons.Inline(Icons.Jump)}
		   {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, {Icons.Inline(Icons.Targets)}all enemies within 3 hexes, {Icons.Inline(Icons.Push)}2, {Icons.Inline(Icons.GetCondition(Conditions.Muddle))}
		   """;

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) => FirstForm
		?
		[
			new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +1)
				.WithPierce(3)
				.WithAfterAttackPerformedSubscription(
					ScenarioEvents.AfterAttackPerformed.Subscription.New(
						applyFunction: async parameters =>
						{
							foreach(Figure figure in RangeHelper.GetFiguresInRange(parameters.AbilityState.Target.Hex, 1).Where(figure =>
								        figure.EnemiesWith(parameters.Performer) && figure != parameters.AbilityState.Target))
							{
								await AbilityCmd.SufferDamage(parameters.AbilityState, figure, 1);
							}
						}))),
			new MonsterAbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					if(state.Performer.Hex.TryGetHexObjectOfType(out Obstacle obstacle))
					{
						await AbilityCmd.TryDestroyObstacle(obstacle);
					}
				}))
		]
		:
		[
			new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +1).WithMoveType(MoveType.Jump)),
			new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0)
				.WithRange(5)
				.WithMinRange(4)
				.WithConditions(Conditions.Poison1)),
			new MonsterAbilityCardAbility(ConditionAbility.Builder().WithConditions(Conditions.Strengthen).WithRange(100)
				.WithTarget(Target.Allies | Target.TargetAll))
		];

	public IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) => FirstForm
		?
		[
			new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -2)),
			new MonsterAbilityCardAbility(ShieldAbility.Builder().WithShieldValue(monster.Stats.CustomValue)),
			new MonsterAbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(monster.Stats.CustomValue)),
			new MonsterAbilityCardAbility(HealAbility.Builder().WithHealValue(monster.Stats.CustomValue)),
			new MonsterAbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					if(state.Performer.Hex.TryGetHexObjectOfType(out Obstacle obstacle))
					{
						await AbilityCmd.TryDestroyObstacle(obstacle);
					}
				}))
		]
		:
		[
			new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, -2).WithMoveType(MoveType.Jump)),
			new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -1)
				.WithRange(3)
				.WithRangeType(RangeType.Melee)
				.WithTarget(Target.Enemies | Target.TargetAll)
				.WithPush(2)
				.WithConditions(Conditions.Muddle))
		];
}
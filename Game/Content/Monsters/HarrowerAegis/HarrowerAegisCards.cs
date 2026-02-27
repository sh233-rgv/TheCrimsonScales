using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public abstract class HarrowerAegisAbilityCard : MonsterAbilityCardModel
{
	public override string CardsAtlasPath => "res://Content/Monsters/HarrowerAegis/Cards.jpg";

	public static IEnumerable<MonsterAbilityCardModel> Deck { get; } =
	[
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>(),
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>(),
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>(),
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>(),
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>(),
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>(),
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>(),
		ModelDB.MonsterAbilityCard<HarrowerAegisAbilityCard7>()
	];
}

public class HarrowerAegisAbilityCard0 : HarrowerAegisAbilityCard
{
	public override int Initiative => 26;
	public override int CardIndex => 0;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(AttackAbility(monster, +2, pierce: 2, push: 2, conditions: [Conditions.Immobilize])),
		new MonsterAbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(1).WithRange(3).Build())
	];

	public override IEnumerable<CardElementInfusion> ElementInfusions { get; } =
		[CardElementInfusion.Infuse(Element.Earth)];
}

public class HarrowerAegisAbilityCard1 : HarrowerAegisAbilityCard
{
	public override int Initiative => 29;
	public override int CardIndex => 1;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(GrantAbility.Builder()
			.WithAbilities(
			[
				RetaliateAbility.Builder().WithRetaliateValue(3).WithRange(3).WithMinRange(2).Build()
			])
			.WithTarget(Target.Allies | Target.TargetAll)
			.WithRange(3)
			.Build()),
		new MonsterAbilityCardAbility(ConditionAbility.Builder()
			.WithConditions(Conditions.Muddle)
			.WithTarget(Target.Enemies | Target.TargetAll)
			.WithRange(3)
			.Build())
	];

	public override IEnumerable<CardElementInfusion> ElementInfusions { get; } =
		[CardElementInfusion.ConsumeWild(Element.Fire)];
}

public class HarrowerAegisAbilityCard2 : HarrowerAegisAbilityCard
{
	public override int Initiative => 33;
	public override int CardIndex => 2;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(HealAbility.Builder()
			.WithHealValue(5)
			.WithTarget(Target.Allies)
			.WithRange(3)
			.WithPull(2)
			.WithConditions(Conditions.Immobilize)
			.Build()),
		new MonsterAbilityCardAbility(GrantAbility.Builder()
			.WithAbilities(
			[
				RetaliateAbility.Builder().WithRetaliateValue(1).Build()
			])
			.WithCustomGetTargets((state, figures) =>
			{
				figures.Add(state.ActionState.GetAbilityState<HealAbility.State>(0).Target);
			})
			.WithConditionalAbilityCheck(ConsumeElementAbilityCheck<GrantAbility.State>([Element.Earth]))
			.Build())
	];

	public override IEnumerable<CardElementConsumption> ElementConsumptions { get; } =
		[CardElementConsumption.Consume(Element.Earth)];
}

public class HarrowerAegisAbilityCard3 : HarrowerAegisAbilityCard
{
	public override int Initiative => 41;
	public override int CardIndex => 3;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(ConditionAbility.Builder()
			.WithConditions(Conditions.Immobilize)
			.WithTargets(3)
			.WithRange(3)
			.WithAbilityPerformedSubscription(
				ConsumeElementCheckSubscription<ScenarioEvents.AbilityPerformed.Parameters>(monster, [Element.Fire],
					applyFunction: async parameters =>
					{
						foreach(Figure figure in ((ConditionAbility.State)parameters.AbilityState).UniqueTargetedFigures)
						{
							await AbilityCmd.SufferDamage(parameters.AbilityState, figure, 1);
						}
					}
				))
			.Build())
	];

	public override IEnumerable<CardElementConsumption> ElementConsumptions { get; } =
		[CardElementConsumption.Consume(Element.Fire)];
}

public class HarrowerAegisAbilityCard4 : HarrowerAegisAbilityCard
{
	public override int Initiative => 50;
	public override int CardIndex => 4;
	public override bool Reshuffles => true;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0)),
		new MonsterAbilityCardAbility(ConditionAbility.Builder()
			.WithConditions(Conditions.Ward)
			.WithTarget(Target.SelfOrAllies | Target.TargetAll)
			.WithRange(1)
			.Build())
	];

	public override IEnumerable<CardElementInfusion> ElementInfusions { get; } =
		[CardElementInfusion.Infuse(Element.Fire), CardElementInfusion.Infuse(Element.Earth)];
}

public class HarrowerAegisAbilityCard5 : HarrowerAegisAbilityCard
{
	public override int Initiative => 63;
	public override int CardIndex => 5;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				Condition poison = state.Performer.Conditions.FirstOrDefault(condition => condition.ConditionModel is Poison);
				if(poison != null)
				{
					await AbilityCmd.RemoveCondition(poison);
				}
			})
			.Build()),
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0, conditions: [Conditions.Poison1])),
		new MonsterAbilityCardAbility(HealAbility.Builder().WithHealValue(3).WithTarget(Target.Self).Build())
	];

	public override IEnumerable<CardElementInfusion> ElementInfusions { get; } =
		[CardElementInfusion.Infuse(Element.Earth)];
}

public class HarrowerAegisAbilityCard6 : HarrowerAegisAbilityCard
{
	public override int Initiative => 73;
	public override int CardIndex => 6;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(AttackAbility(monster, -1, range: 3, targets: 3,
			afterAttackPerformedSubscriptions:
			[
				ScenarioEvents.AfterAttackPerformed.Subscription.New(
					applyFunction: async parameters =>
					{
						await AbilityCmd.SufferDamage(parameters.AbilityState, parameters.AbilityState.Target, 1);
					})
			]))
	];

	public override IEnumerable<CardElementInfusion> ElementInfusions { get; } =
		[CardElementInfusion.Infuse(Element.Fire)];
}

public class HarrowerAegisAbilityCard7 : HarrowerAegisAbilityCard
{
	public override int Initiative => 88;
	public override int CardIndex => 7;

	public override bool Reshuffles => true;

	//TODO: Allow for choosing path
	public override Action<ScenarioCheckEvents.FigureFocus.Parameters> AdjustFocus => parameters => parameters.SetFocusFarthest();

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +2, MoveType.Jump)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0,
			target: Target.Enemies | Target.TargetAll,
			customGetTargets: (state, figures) =>
			{
				figures.AddRange(state.ActionState.GetAbilityState<MoveAbility.State>(0).Hexes.SelectMany(hex => RangeHelper.GetHexesInRange(hex, 1))
					.SelectMany(hex => hex.GetHexObjectsOfType<Figure>()));
			}, duringAttackSubscriptions:
			[
				ConsumeElementCheckSubscription<ScenarioEvents.DuringAttack.Parameters>(monster, [Element.Fire, Element.Earth],
					applyFunction: async parameters =>
					{
						parameters.AbilityState.AbilityAddCondition(Conditions.Immobilize);
						await GDTask.CompletedTask;
					}
				)
			])),
	];

	public override IEnumerable<CardElementConsumption> ElementConsumptions { get; } =
		[CardElementConsumption.Consume([Element.Fire, Element.Earth])];
}
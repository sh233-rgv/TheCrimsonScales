using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public abstract class RavenousGharialAbilityCard : MonsterAbilityCardModel
{
	public override string CardsAtlasPath => "res://Content/Monsters/RavenousGharial/Cards.jpg";

	public static IEnumerable<MonsterAbilityCardModel> Deck { get; } =
	[
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard0>(),
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard1>(),
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard2>(),
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard3>(),
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard4>(),
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard5>(),
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard6>(),
		ModelDB.MonsterAbilityCard<RavenousGharialAbilityCard7>()
	];
}

public class RavenousGharialAbilityCard0 : RavenousGharialAbilityCard
{
	public override int Initiative => 30;
	public override int CardIndex => 0;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(AttackAbility(monster, -2).WithConditions(Conditions.Immobilize)),
		new MonsterAbilityCardAbility(TriggeredAbility.Builder()
			.WithAbilities(AttackAbility(monster, -1).WithConditions(Conditions.Wound1))
			.WithInitiative(60)),
		new MonsterAbilityCardAbility(TriggeredAbility.Builder()
			.WithAbilities(AttackAbility(monster, +0))
			.WithInitiative(90))
	];
}

public class RavenousGharialAbilityCard1 : RavenousGharialAbilityCard
{
	public override int Initiative => 65;
	public override int CardIndex => 1;
	public override bool Reshuffles => true;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0)),
		new MonsterAbilityCardAbility(HealAbility.Builder().WithHealValue(new DynamicInt<HealAbility.State>(state =>
			state.ActionState.GetAbilityState<AttackAbility.State>(1).DamageDealt)).WithTarget(Target.Self))
	];
}

public class RavenousGharialAbilityCard2 : RavenousGharialAbilityCard
{
	public override int Initiative => 59;
	public override int CardIndex => 2;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0).WithConditions(Conditions.Immobilize)),
		new MonsterAbilityCardAbility(PushAbility.Builder()
			.WithPush(1)
			.WithTarget(Target.TargetAll | Target.Enemies)
			.WithRange(3)
			.WithMinRange(2))
	];
}

public class RavenousGharialAbilityCard3 : RavenousGharialAbilityCard
{
	public override int Initiative => 34;
	public override int CardIndex => 3;
	public override bool Reshuffles => true;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, -1).WithRange(2).WithPull(1).WithConditions(Conditions.Wound1))
	];
}

public class RavenousGharialAbilityCard4 : RavenousGharialAbilityCard
{
	public override int Initiative => 88;
	public override int CardIndex => 4;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +1)
			.WithOnAbilityStarted(async abilityState =>
			{
				ScenarioCheckEvents.MoveCheckEvent.Subscribe(abilityState, this,
					canApplyParameters =>
						canApplyParameters.AbilityState == abilityState && canApplyParameters.Hex.HasHexObjectOfType<Water>(),
					applyParameters =>
					{
						applyParameters.SetMoveCost(1);
					}
				);

				await GDTask.CompletedTask;
			})
			.WithOnAbilityEnded(async abilityState =>
				{
					ScenarioCheckEvents.MoveCheckEvent.Unsubscribe(abilityState, this);

					await GDTask.CompletedTask;
				}
			)),
		new MonsterAbilityCardAbility(AttackAbility(monster, -1).WithPush(2).WithAOEPattern(new AOEPattern(
		[
			new AOEHex(Vector2I.Zero, AOEHexType.Gray),
			new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
			new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
			new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red)
		])))
	];
}

public class RavenousGharialAbilityCard5 : RavenousGharialAbilityCard
{
	public override int Initiative => 09;
	public override int CardIndex => 5;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(monster.Stats.Attack)),
		new MonsterAbilityCardAbility(OtherActiveAbility.Builder()
			.WithOnActivate(async _ =>
			{
				ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(monster, this,
					parameters => parameters.AbilityState.Target == monster && parameters.AbilityState.SingleTargetRangeType == RangeType.Range,
					async parameters =>
					{
						parameters.AbilityState.SingleTargetSetHasDisadvantage();
						await GDTask.CompletedTask;
					});
				await GDTask.CompletedTask;
			})
			.WithOnDeactivate(async _ =>
			{
				ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(monster, this);
				await GDTask.CompletedTask;
			}))
	];
}

public class RavenousGharialAbilityCard6 : RavenousGharialAbilityCard
{
	public override int Initiative => 18;
	public override int CardIndex => 6;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(ConditionAbility.Builder().WithConditions(Conditions.Invisible).WithTarget(Target.Self)),
		new MonsterAbilityCardAbility(TriggeredAbility.Builder()
			.WithAbilities(
				MoveAbility(monster, +0),
				AttackAbility(monster, +1)
					.WithOnAbilityEndedPerformed(async state =>
					{
						await AbilityCmd.RemoveCondition(state.Performer, Conditions.Invisible);
					}))
			.WithInitiative(50))
	];
}

public class RavenousGharialAbilityCard7 : RavenousGharialAbilityCard
{
	public override int Initiative => 26;
	public override int CardIndex => 7;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0).WithPierce(3))
	];
}
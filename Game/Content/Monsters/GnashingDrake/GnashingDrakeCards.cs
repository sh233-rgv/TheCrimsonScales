using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public abstract class GnashingDrakeAbilityCard : MonsterAbilityCardModel
{
	public override string CardsAtlasPath => "res://Content/Monsters/GnashingDrake/Cards.jpg";

	public static IEnumerable<MonsterAbilityCardModel> Deck { get; } =
	[
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard0>(),
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard1>(),
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard2>(),
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard3>(),
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard4>(),
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard5>(),
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard6>(),
		ModelDB.MonsterAbilityCard<GnashingDrakeAbilityCard7>()
	];
}

public class GnashingDrakeAbilityCard0 : GnashingDrakeAbilityCard
{
	public override int Initiative => 06;
	public override int CardIndex => 0;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(ShieldAbility.Builder().WithShieldValue(2).Build()),
		new MonsterAbilityCardAbility(HealAbility.Builder().WithHealValue(2).WithTarget(Target.Self).Build()),
		new MonsterAbilityCardAbility(ConditionAbility.Builder().WithConditions(Conditions.Strengthen).Build())
	];
}

public class GnashingDrakeAbilityCard1 : GnashingDrakeAbilityCard
{
	public override int Initiative => 18;
	public override int CardIndex => 1;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, -1))
	];
}

public class GnashingDrakeAbilityCard2 : GnashingDrakeAbilityCard
{
	public override int Initiative => 24;
	public override int CardIndex => 2;
	public override bool Reshuffles => true;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(AttackAbility(monster, -1, conditions: [Conditions.Wound1])),
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)
			/*.WithOnAbilityStarted(async state =>
			{
				state.ActionState.ClearFocus();
				ScenarioCheckEvents.FigureFocusCheckEvent.Subscribe(state, this,
					parameters => parameters.AbilityState == state,
					parameters =>
					{
						parameters.SetFocusFarthest();
					});
				await GDTask.CompletedTask;
			})
			.WithOnAbilityEnded(async state =>
			{
				ScenarioCheckEvents.FigureFocusCheckEvent.Unsubscribe(state, this);
				await GDTask.CompletedTask;
			})*/),
		//TODO: Add once onabilityended allowed
		new MonsterAbilityCardAbility(AttackAbility(monster, -1, conditions: [Conditions.Wound1])),
	];
}

public class GnashingDrakeAbilityCard3 : GnashingDrakeAbilityCard
{
	public override int Initiative => 30;
	public override int CardIndex => 3;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0, afterTargetConfirmedSubscriptions:
		[
			ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
				parameters => parameters.AbilityState.Target.IsDamaged(),
				async parameters =>
				{
					parameters.AbilityState.SingleTargetAdjustAttackValue(1);
					await GDTask.CompletedTask;
				})
		]))
	];
}

public class GnashingDrakeAbilityCard4 : GnashingDrakeAbilityCard
{
	public override int Initiative => 35;
	public override int CardIndex => 4;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, -1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, -1, conditions: [Conditions.Poison1],
			aoePattern: new AOEPattern([
				new AOEHex(Vector2I.Zero, AOEHexType.Gray),
				new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red),
			])))
	];
}

public class GnashingDrakeAbilityCard5 : GnashingDrakeAbilityCard
{
	public override int Initiative => 44;
	public override int CardIndex => 5;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(AttackAbility(monster, -1, range: 3, targets: 2,
			afterTargetConfirmedSubscriptions:
			[
				ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
					parameters => parameters.AbilityState.Target.IsDamaged(),
					async parameters =>
					{
						parameters.AbilityState.SingleTargetAdjustAttackValue(1);
						await GDTask.CompletedTask;
					})
			]))
	];
}

public class GnashingDrakeAbilityCard6 : GnashingDrakeAbilityCard
{
	public override int Initiative => 62;
	public override int CardIndex => 6;
	public override bool Reshuffles => true;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, -1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, -1, range: 2, conditions: [Conditions.Muddle],
			aoePattern: new AOEPattern([
				new AOEHex(Vector2I.Zero, AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red)
			])))
	];
}

public class GnashingDrakeAbilityCard7 : GnashingDrakeAbilityCard
{
	public override int Initiative => 73;
	public override int CardIndex => 7;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, -1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +1))
	];
}
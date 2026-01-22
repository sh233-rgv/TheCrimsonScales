using System.Collections.Generic;
using Fractural.Tasks;

public class UltravioletRays : BrightsparkCardModel<UltravioletRays.CardTop, UltravioletRays.CardBottom>
{
	public override string Name => "Ultraviolet Rays";
	public override int Level => 9;
	public override int Initiative => 66;
	protected override int AtlasIndex => 29;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					object subscriber1 = new object();
					object subscriber2 = new object();
					object subscriber3 = new object();
					object subscriber4 = new object();
					List<object> subscribers = [subscriber1, subscriber2, subscriber3, subscriber4];
					state.SetCustomValue(this, "subscribers", subscribers);

					ScenarioEvents.DuringAttackEvent.Subscribe(ScenarioEvents.GetSubscriberPair(state, subscriber1),
						ScenarioEvents.DuringAttack.Subscription.ConsumeWildElements(
							parameters => parameters.AbilityState.IsSingleTarget,
							async applyParameters =>
							{
								applyParameters.AbilityState.SingleTargetAdjustAttackValue(2);
								await GDTask.CompletedTask;
							}));
					ScenarioEvents.DuringAttackEvent.Subscribe(ScenarioEvents.GetSubscriberPair(state, subscriber1),
						ScenarioEvents.DuringAttack.Subscription.ConsumeWildElements(
							parameters => parameters.AbilityState.IsSingleTarget,
							async applyParameters =>
							{
								applyParameters.AbilityState.SingleTargetAdjustPierce(3);
								await GDTask.CompletedTask;
							}));
					ScenarioEvents.DuringAttackEvent.Subscribe(ScenarioEvents.GetSubscriberPair(state, subscriber1),
						ScenarioEvents.DuringAttack.Subscription.ConsumeWildElements(
							parameters => parameters.AbilityState.IsSingleTarget,
							async applyParameters =>
							{
								applyParameters.AbilityState.SingleTargetAdjustPush(2);
								await GDTask.CompletedTask;
							}));
					ScenarioEvents.DuringAttackEvent.Subscribe(ScenarioEvents.GetSubscriberPair(state, subscriber1),
						ScenarioEvents.DuringAttack.Subscription.ConsumeWildElements(
							parameters => parameters.AbilityState.IsSingleTarget,
							async applyParameters =>
							{
								applyParameters.AbilityState.SingleTargetAddCondition(Conditions.Poison1);
								await GDTask.CompletedTask;
							}));
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					foreach(object obj in state.GetCustomValue<List<object>>(this, "subscribers"))
					{
						ScenarioEvents.DuringAttackEvent.Unsubscribe(state, obj);
					}

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 3;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.WithDuringMovementSubscriptions(
					[
						ScenarioEvents.DuringMovement.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								parameters.AbilityState.AdjustMoveValue(3);

								await GDTask.CompletedTask;
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"+3{Icons.Inline(Icons.Move)}")
						),
						ScenarioEvents.DuringMovement.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								parameters.AbilityState.AdjustMoveValue(3);

								await GDTask.CompletedTask;
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"+3{Icons.Inline(Icons.Move)}")
						)
					]
				)
				.Build())
		];

		//TODO: Elements: 2x wild element
	}
}
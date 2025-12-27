using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class CriticalObservation : BrightsparkCardModel<CriticalObservation.CardTop, CriticalObservation.CardBottom>
{
	public override string Name => "Critical Observation";
	public override int Level => 1;
	public override int Initiative => 20;
	protected override int AtlasIndex => 5;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithAfterTargetConfirmedSubscription(
					ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
						parameters => Math.Abs(parameters.AbilityState.Target.Initiative.MainInitiative -
						                       parameters.Performer.Initiative.MainInitiative) <= 15,
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAdjustAttackValue(1);
							await AbilityCmd.InfuseElement(Element.Light);
							await AbilityCmd.GainXP(parameters.Performer, 1);
						})
				)
				.Build()),
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer,
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAddCondition(state.UseSlotIndex switch
							{
								0 => Conditions.Muddle,
								1 => Conditions.Poison1,
								2 => Conditions.Wound1,
								_ => null
							});
							await state.AdvanceUseSlot();
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					//TODO: Fix Use slot positioning
					new UseSlot(new Vector2(0.16650043f, 0.3549993f)),
					new UseSlot(new Vector2(0.57749975f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f))
				])
				.Build())
		];

		protected override bool Persistent => true;
	}
}
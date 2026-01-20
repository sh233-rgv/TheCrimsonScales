using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class CriticalHypothesis : BrightsparkCardModel<CriticalHypothesis.CardTop, CriticalHypothesis.CardBottom>
{
	public override string Name => "Critical Hypothesis";
	public override int Level => 8;
	public override int Initiative => 16;
	protected override int AtlasIndex => 26;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4)
				.WithAfterTargetConfirmedSubscription(
					ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
						parameters => Math.Abs(parameters.AbilityState.Target.Initiative.MainInitiative -
						                       parameters.Performer.Initiative.MainInitiative) <= 10,
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAddCondition(Conditions.Stun);
							//TODO: Add state
							await AbilityCmd.InfuseWildElement(parameters.AbilityState);
							await AbilityCmd.GainXP(parameters.Performer, 1);
						})
				)
				.Build()),
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
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
								0 => Conditions.Wound1,
								1 => Conditions.Disarm,
								2 => Conditions.Stun,
								_ => throw new ArgumentOutOfRangeException()
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
					//TODO: Fix use slot positioning
					new UseSlot(new Vector2(0.2925002f, 0.8124983f)),
					new UseSlot(new Vector2(0.50000006f, 0.8124983f)),
					new UseSlot(new Vector2(0.70749974f, 0.8124983f))
				])
				.Build())
		];

		public override bool Persistent => true;
	}
}
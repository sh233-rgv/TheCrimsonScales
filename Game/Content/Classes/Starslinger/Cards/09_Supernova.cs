using System.Collections.Generic;
using Godot;
using Fractural.Tasks;

public class Supernova : StarslingerCardModel<Supernova.CardTop, Supernova.CardBottom>
{
	public override string Name => "Supernova";
	public override int Level => 1;
	public override int Initiative => 30;
	protected override int AtlasIndex => 9;

	public class CardTop : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithRange(3)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New(
						parameters => !parameters.Performer.IsDamaged(),
						async parameters =>
						{
							((AttackAbility.State)parameters.AbilityState).AbilityAdjustAttackValue(1);

							await GDTask.CompletedTask;
						}
					)
				)
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Light,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AdjustTargets(1);
							parameters.AbilityState.AbilityAdjustRange(-1);

							await AbilityCmd.GainXP(parameters.Performer, 1);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Attack)}, -1{Icons.Inline(Icons.Range)}")
					)
				)
				.Build())
		];
	}

	public class CardBottom : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						parameters =>
							parameters.Performer == state.Performer && !parameters.Performer.IsDamaged(),
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAdjustAttackValue(2);
							if(state.UseSlotIndex == 1)
							{
								await AbilityCmd.InfuseElement(Element.Light);
							}

							await state.AdvanceUseSlot();
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					new UseSlot(new Vector2(0.2889934f, 0.38399956f), GainXP),
					new UseSlot(new Vector2(0.5f, 0.38399956f)),
					new UseSlot(new Vector2(0.7025001f, 0.38399956f), GainXP)
					//TODO: Fix use slot positions
				])
				.Build())
		];

		protected override bool Persistent => true;
		protected override bool Loss => true;
	}
}
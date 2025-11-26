using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class PhotonicDefense : LuminaryCardModel<PhotonicDefense.CardTop, PhotonicDefense.CardBottom>
{
	public override string Name => "Photonic Defense";
	public override int Level => 1;
	public override int Initiative => 09;
	protected override int AtlasIndex => 25;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						canApply: parameters => state.Performer == parameters.AbilityState.Target,
						apply: async parameters =>
						{
							ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription shieldChosenSubscription =
								ScenarioEvents.GenericChoice.Subscription.New(
									applyFunction: async applyParameters =>
									{
										ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
											canApplyParameters => canApplyParameters.Figure == state.Performer && canApplyParameters.FromAttack,
											async applyParameters =>
											{
												applyParameters.AdjustShield(3);
												ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);

												await GDTask.CompletedTask;
											}
										);
										
										await GDTask.CompletedTask;
									},
									effectButtonParameters: new IconEffectButton.Parameters(Icons.Shield),
									effectInfoViewParameters: new TextEffectInfoView.Parameters($"Gain {Icons.Inline(Icons.Shield)}3 for the attack"),
									effectType: EffectType.SelectableMandatory
								);

							ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription retaliateChosenSubscription =
								ScenarioEvents.GenericChoice.Subscription.New(
									applyFunction: async applyParameters =>
									{
										ScenarioEvents.RetaliateEvent.Subscribe(state, this,
											canApplyParameters =>
											{
												return canApplyParameters.RetaliatingFigure == state.Performer &&
													RangeHelper.Distance(canApplyParameters.AbilityState.Performer.Hex, state.Performer.Hex) <= 1;
											},
											async applyParameters =>
											{
												applyParameters.AdjustRetaliate(3);
												ScenarioEvents.RetaliateEvent.Unsubscribe(state, this);

												await GDTask.CompletedTask;
											}
										);

										await GDTask.CompletedTask;
									},
									effectButtonParameters: new IconEffectButton.Parameters(Icons.Retaliate),
									effectInfoViewParameters: new TextEffectInfoView.Parameters($"Gain {Icons.Inline(Icons.Retaliate)}3 for the attack"),
									effectType: EffectType.SelectableMandatory
								);

							await AbilityCmd.GenericChoice(state.Performer, 
								[shieldChosenSubscription, retaliateChosenSubscription], 
								hintText: "Select an effect to gain for the attack:");

							switch(state.UseSlotIndex)
							{
								case 0:
									await AbilityCmd.InfuseElement(Element.Fire, state.Authority);
									break;
								case 1:
									await AbilityCmd.InfuseElement(Element.Ice, state.Authority);
									break;
								case 2:
									await AbilityCmd.InfuseElement(Element.Light, state.Authority);
									break;
								case 3:
									await AbilityCmd.InfuseElement(Element.Dark, state.Authority);
									break;
							}

							await state.AdvanceUseSlot();
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);
					ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);
					ScenarioEvents.RetaliateEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					new UseSlot(new Vector2(0.2869934f, 0.30899984f)),
					new UseSlot(new Vector2(0.70750487f, 0.30899984f)),
					new UseSlot(new Vector2(0.603f, 0.43299824f)),
					new UseSlot(new Vector2(0.39799652f, 0.43299824f), Gain2XP)
					//TODO: FIx positioning
				])
				.Build())
		];
		
		protected override bool Persistent => true;
		protected override bool Loss => true;
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					int consumedElements = 0;
					for(int i = 0; i < 6; i++)
					{
						if (await AbilityCmd.TryConsumeElement((Element)i))
                        {
							consumedElements++;
							state.SetPerformed();
                        }
					}
					state.SetCustomValue(this, "ConsumedElements", consumedElements);

					await GDTask.CompletedTask;
				})
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(0)
				.WithTargets(0)
				.WithRange(3)
				.WithOnAbilityStarted(async state =>
                {
					int consumedElements = state.ActionState.GetAbilityState<OtherAbility.State>(0).GetCustomValue<int>(this, "ConsumedElements");
                    state.AbilityAdjustAttackValue(consumedElements);
					state.AdjustTargets(consumedElements);

					await GDTask.CompletedTask;
                })
				.Build()),
		];

		protected override int XP => 2;
		protected override bool Loss => true;
	}
}
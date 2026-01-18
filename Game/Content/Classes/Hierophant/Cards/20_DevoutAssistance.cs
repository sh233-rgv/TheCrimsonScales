using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class DevoutAssistance : HierophantLevelUpCardModel<DevoutAssistance.CardTop, DevoutAssistance.CardBottom>
{
	public override string Name => "Devout Assistance";
	public override int Level => 5;
	public override int Initiative => 37;
	protected override int AtlasIndex => 15 - 6;

	public class CardTop : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnStartedEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Figure == state.Performer,
						async parameters =>
						{
							ActionState actionState = new ActionState(state.Performer,
							[
								GrantAbility.Builder()
									.WithGetAbilities(grantAbilityState =>
									[
										ShieldAbility.Builder()
											.WithShieldValue(1)
											.WithOnAbilityStarted(async shieldAbilityState =>
											{
												await AbilityCmd.GenericChoice(shieldAbilityState.Performer,
												[
													ScenarioEvents.GenericChoice.Subscription.ConsumeElement(Element.Earth,
														applyFunction: async applyParameters =>
														{
															shieldAbilityState.SetCustomValue(this, "ChoseShield", true);
															await GDTask.CompletedTask;
														},
														effectInfoViewParameters: new TextEffectInfoView.Parameters(
															$"Perform {Icons.Inline(Icons.Shield)}1"),
														effectType: EffectType.SelectableMandatory
													),
													ScenarioEvents.GenericChoice.Subscription.ConsumeElement(Element.Light,
														applyFunction: async applyParameters =>
														{
															shieldAbilityState.SetBlocked();
															await GDTask.CompletedTask;
														},
														effectInfoViewParameters: new TextEffectInfoView.Parameters(
															$"Perform {Icons.Inline(Icons.Heal)}3, self"),
														effectType: EffectType.SelectableMandatory
													)
												], hintText: "Select an ability to perform:");
											})
											.WithOnAbilityEndedPerformed(async shieldAbilityState =>
											{
												ScenarioEvents.RoundEndedEvent.Subscribe(shieldAbilityState, this,
													parameters => true,
													async parameters =>
													{
														await shieldAbilityState.RemoveFromActive();
													});
												await GDTask.CompletedTask;
											})
											.Build(),

										HealAbility.Builder()
											.WithHealValue(3)
											.WithTarget(Target.Self)
											.WithConditionalAbilityCheck(async healAbilityState =>
											{
												await GDTask.CompletedTask;
												return !healAbilityState.ActionState.GetAbilityState<ShieldAbility.State>(0)
													.GetCustomValue<bool>(this, "ChoseShield");
											})
											.Build()
									])
									.Build()
							]);
							await actionState.Perform();
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.FigureTurnStartedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		protected override int XP => 2;
		protected override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
				.WithOnAbilityEndedPerformed(async state =>
				{
					if(RangeHelper.GetFiguresInRange(state.Performer.Hex, 1, false).Any(figure => figure.EnemiesWith(state.Performer)))
					{
						await AbilityCmd.InfuseElement(state, Element.Earth);
					}

					if(RangeHelper.GetFiguresInRange(state.Performer.Hex, 1, false).Any(figure => figure.AlliedWith(state.Performer)))
					{
						await AbilityCmd.InfuseElement(state, Element.Light);
					}
				})
				.Build()),
		];
	}
}
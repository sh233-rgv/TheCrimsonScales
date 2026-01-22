using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ElixirOfLife : BrightsparkCardModel<ElixirOfLife.CardTop, ElixirOfLife.CardBottom>
{
	public override string Name => "Elixir of Life";
	public override int Level => 9;
	public override int Initiative => 38;
	protected override int AtlasIndex => 28;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(GiveAbilityCardAbility.Builder()
				.WithGetAbilityCards((state, list) =>
				{
					list.Add(GetAbilityCard(state));
				})
				.WithOnCardGiven(OnCardGiven)
				.WithOnCardDiscarded(OnCardDiscarded)
				.WithOnCardLost(OnCardLost)
				.WithSelectAutomatically(true)
				.Build()
			),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					Figure target = state.ActionState.GetAbilityState<GiveAbilityCardAbility.State>(0).UniqueTargetedFigures[0];

					//TODO: Before Killed Event (needs mirefoot L9)


					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					//TODO: ScenarioEvents.BeforeKilledEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build())
		];

		public override bool Persistent => true;
		public override bool Unrecoverable => true;
		public override bool Loss => true;

		private async GDTask OnCardGiven(AbilityState abilityState, AbilityCard abilityCard)
		{
			Character originalOwner = GetOriginalOwner(abilityState);
			originalOwner.RemoveCard(abilityCard);

			await GDTask.CompletedTask;
		}

		private async GDTask OnCardDiscarded(AbilityCard abilityCard)
		{
			abilityCard.Owner.RemoveCard(abilityCard);

			Character originalOwner = abilityCard.OriginalOwner;
			originalOwner.AddCard(abilityCard);

			await GDTask.CompletedTask;
		}

		private async GDTask OnCardLost(AbilityCard abilityCard)
		{
			abilityCard.Owner.RemoveCard(abilityCard);

			Character originalOwner = abilityCard.OriginalOwner;
			originalOwner.AddCard(abilityCard);

			await GDTask.CompletedTask;
		}
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(3)
				.WithRange(3)
				.WithOnAbilityEndedPerformed(async state =>
				{
					await AbilityCmd.GainXP(state.Performer, 1);
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.AskConsumeElement(state.Performer, Element.Light))
				.Build()),
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer,
						async parameters =>
						{
							int healValue = (state.UseSlotIndex + 3) / 2;
							int moveValue = 2 - state.UseSlotIndex % 2;
							ActionState actionState = new ActionState(parameters.Figure,
							[
								MoveAbility.Builder().WithDistance(healValue).Build(),
								HealAbility.Builder().WithHealValue(moveValue).WithTarget(Target.Self).Build()
							]);
							await actionState.Perform();

							await state.AdvanceUseSlot();
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					//TODO: Use Slot Positioning
					new UseSlot(new Vector2(0.78700954f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f)),
				])
				.Build()),
		];

		public override bool Persistent => true;
	}
}
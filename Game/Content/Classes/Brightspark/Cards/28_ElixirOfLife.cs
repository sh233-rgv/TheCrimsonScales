using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ElixirOfLife : BrightsparkCardModel<ElixirOfLife.CardTop, ElixirOfLife.CardBottom>
{
	public override string Name => "Elixir Of Life";
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

					ScenarioEvents.InflictConditionEvent.Subscribe(state, this,
						parameters =>
							parameters.Target == target &&
							AbilityCmd.CheckImmunity(parameters.ConditionModel, Conditions.Poison1),
						async parameters =>
						{
							parameters.SetPrevented(true);

							ActionState actionState =
								new ActionState(target, [HealAbility.Builder().WithHealValue(1).WithTarget(Target.Self).Build()]);
							await actionState.Perform();
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.InflictConditionEvent.Unsubscribe(state, this);

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
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer,
						async parameters =>
						{
							await AbilityCmd.InfuseWildElement(state);
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
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP),
					new UseSlot(new Vector2(0.16650043f, 0.3549993f), async state => await AbilityCmd.InfuseElement(state, Element.Fire)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP),
					new UseSlot(new Vector2(0.57749975f, 0.3549993f), async state => await AbilityCmd.InfuseWildElement(state)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), async state =>
					{
						await AbilityCmd.InfuseWildElement(state);
						await AbilityCmd.InfuseWildElement(state);
					})
				])
				.Build()),
		];

		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
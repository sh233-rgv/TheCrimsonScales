using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public abstract class LuminaryCardModel<TTop, TBottom> : AtlasAbilityCardModel<TTop, TBottom>
	where TTop : LuminaryCardSide, new()
	where TBottom : LuminaryCardSide, new()
{
	protected override string TexturePath => "res://Content/Classes/Luminary/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class LuminaryCardSide : AbilityCardSide
{
	protected AbilityCardAbility Scuttle(int distance, IReadOnlyCollection<Element> possibleElements)
	{
		return new AbilityCardAbility(MoveAbility.Builder()
			.WithDistance(distance)
			.WithMoveType(MoveType.Jump)
			.WithConditionalAbilityCheck(async state =>
				{
					await GDTask.CompletedTask;

					return state.ActionState.GetAbilityState<AttackAbility.State>(0).Performed;
				})
			.WithOnAbilityEndedPerformed(async state =>
			{
				if(possibleElements.Count == 1)
				{
					await AbilityCmd.InfuseElement(possibleElements.ToList()[0]);
				}
				else
				{
					await AbilityCmd.InfuseElement(state.Performer, possibleElements);
				}
			})
			.WithOnAbilityStarted(async state =>
			{
			ScenarioCheckEvents.MoveCanStopAtCheckEvent.Subscribe(state.Performer, this,
				parameters =>
					parameters.AbilityState == state && !state.ActionState.GetAbilityState<AttackAbility.State>(0).GetEmptyAOEHexes().Contains(parameters.Hex),
					parameters =>
					{
						parameters.SetCannotStopAt();
					}
				);

				await GDTask.CompletedTask;
			})
			.WithOnAbilityEnded(async state =>
				{
					ScenarioCheckEvents.MoveCanStopAtCheckEvent.Unsubscribe(state.Performer, this);

					await GDTask.CompletedTask;
				}
			)
			.Build());
	}

	protected AbilityCardAbility Glow(Element element, Ability ability)
    {
        return new AbilityCardAbility(OtherActiveAbility.Builder()
			.WithOnActivate(async state =>
			{
				foreach(AbilityCard abilityCard in ((Character)state.Performer).Cards)
                {
                    foreach(ActionState actionState in abilityCard.ActiveActionStates)
                    {
                        if(actionState.AbilityStates.Any(abilityState => abilityState.GetCustomValue<bool>("Glow", "Active Glow")))
                        {
                            await actionState.RequestDiscardOrLose();
                        }
                    }
                }

				ScenarioEvents.FigureTurnStartedEvent.Subscribe(state, this,
					canApplyParameters => canApplyParameters.Figure == state.Performer && GameController.Instance.ElementManager.GetState(element) > ElementState.Inert,
					async applyParameters =>
					{
						if(await AbilityCmd.AskConsumeElement(state.Performer, element))
						{
							ActionState actionState = new(state.Performer,[ability]);
							await actionState.Perform();
						}
					},
					EffectType.Selectable,
					canApplyMultipleTimesInEffectCollection: true,
					effectButtonParameters: new IconEffectButton.Parameters("res://Content/Classes/Luminary/Glow.svg"),
					effectInfoViewParameters: new TextEffectInfoView.Parameters($"Perform {Icons.Inline("res://Content/Classes/Luminary/Glow.svg")}"));

				ScenarioEvents.AbilityEndedEvent.Subscribe(state, this,
					canApplyParameters => canApplyParameters.Performer == state.Performer && GameController.Instance.ElementManager.GetState(element) > ElementState.Inert,
					async applyParameters =>
					{
						if(await AbilityCmd.AskConsumeElement(state.Performer, element))
						{
							ActionState actionState = new(state.Performer,[ability]);
							await actionState.Perform();
						}
					},
					EffectType.Selectable,
					canApplyMultipleTimesInEffectCollection: true,
					effectButtonParameters: new IconEffectButton.Parameters("res://Content/Classes/Luminary/Glow.svg"),
					effectInfoViewParameters: new TextEffectInfoView.Parameters($"Perform {Icons.Inline("res://Content/Classes/Luminary/Glow.svg")}"));

				state.SetCustomValue("Glow", "Active Glow", true);
				state.SetCustomValue("Glow", "Glow Perform", ability);
				await GDTask.CompletedTask;
			})
			.WithOnDeactivate(async state =>
			{
				ScenarioEvents.FigureTurnStartedEvent.Unsubscribe(state, this);
				ScenarioEvents.AbilityEndedEvent.Unsubscribe(state, this);

				await GDTask.CompletedTask;
			})
			.Build());
    }
	
}
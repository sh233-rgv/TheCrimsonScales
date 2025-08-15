using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class RetaliateAbility : ActiveAbility<RetaliateAbility.State>
{
	public class State : ActiveAbilityState
	{
		public int RetaliateValue { get; set; }
		public int Range { get; set; }

		public void AdjustRetaliateValue(int amount)
		{
			RetaliateValue += amount;
		}

		public void AdjustRange(int amount)
		{
			Range += amount;
		}
	}

	public int RetaliateValue { get; }
	public int Range { get; }

	public RetaliateAbility(int retaliateValue, int range = 1,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getHintText = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, getHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		RetaliateValue = retaliateValue;
		Range = range;
	}

	protected override void InitializeState(State abilityState)
	{
		base.InitializeState(abilityState);

		abilityState.RetaliateValue = RetaliateValue;
		abilityState.Range = Range;
	}

	protected override async GDTask Perform(State abilityState)
	{
		await AskConfirmAndActivate(abilityState);
	}

	protected override async GDTask Activate(State abilityState)
	{
		await base.Activate(abilityState);

		ScenarioCheckEvents.RetaliateCheckEvent.Subscribe(abilityState, this,
			canApplyParameters =>
				canApplyParameters.Figure == abilityState.Performer,
			applyParameters =>
			{
				applyParameters.AddRetaliate(abilityState.RetaliateValue, abilityState.Range);
			}
		);

		ScenarioEvents.RetaliateEvent.Subscribe(abilityState, this,
			canApplyParameters =>
			{
				bool canApply =
					canApplyParameters.RetaliatingFigure == abilityState.Performer &&
					RangeHelper.Distance(canApplyParameters.AbilityState.Performer.Hex, abilityState.Performer.Hex) <= abilityState.Range;

				return canApply;
			},
			async applyParameters =>
			{
				applyParameters.AdjustRetaliate(abilityState.RetaliateValue);

				await GDTask.CompletedTask;
			}
		);
	}

	protected override async GDTask Deactivate(State abilityState)
	{
		await base.Deactivate(abilityState);

		ScenarioCheckEvents.RetaliateCheckEvent.Unsubscribe(abilityState, this);
		ScenarioEvents.RetaliateEvent.Unsubscribe(abilityState, this);
	}

	protected override string DefaultHintText(State abilityState)
	{
		return $"Perform the {Icons.HintText(Icons.Retaliate)} ability?";
	}
}
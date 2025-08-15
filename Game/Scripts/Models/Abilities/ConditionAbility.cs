using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class ConditionAbility : TargetedAbility<ConditionAbility.State, SingleTargetState>
{
	public class State : TargetedAbilityState<SingleTargetState>
	{
	}

	public List<ScenarioEvent<ScenarioEvents.ConditionAfterTargetConfirmed.Parameters>.Subscription> AfterTargetConfirmedSubscriptions { get; }

	public ConditionAbility(ConditionModel[] conditions, int targets = 1, int? range = null, RangeType? rangeType = null,
		Target? target = null,
		bool requiresLineOfSight = true, bool mandatory = false,
		Hex targetHex = null,
		AOEPattern aoePattern = null, int push = 0, int pull = 0,
		Action<State, List<Figure>> customGetTargets = null,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getTargetingHintText = null,
		List<ScenarioEvents.ConditionAfterTargetConfirmed.Subscription> afterTargetConfirmedSubscriptions = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(targets, range, rangeType, target ?? ((conditions.Length > 0 && conditions[0].IsPositive) ? Target.SelfOrAllies : Target.Enemies),
			requiresLineOfSight, mandatory, targetHex, aoePattern, push, pull, conditions,
			customGetTargets, onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed,
			conditionalAbilityCheck, getTargetingHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		AfterTargetConfirmedSubscriptions = afterTargetConfirmedSubscriptions;
	}

	protected override async GDTask StartPerform(State abilityState)
	{
		await base.StartPerform(abilityState);

		ScenarioEvents.ConditionAfterTargetConfirmedEvent.Subscribe(abilityState, this, AfterTargetConfirmedSubscriptions);
	}

	protected override async GDTask EndPerform(State abilityState)
	{
		await base.EndPerform(abilityState);

		ScenarioEvents.ConditionAfterTargetConfirmedEvent.Unsubscribe(AfterTargetConfirmedSubscriptions);
	}

	protected override async GDTask AfterTargetConfirmedBeforeConditionsApplied(State abilityState, Figure target)
	{
		await ScenarioEvents.ConditionAfterTargetConfirmedEvent.CreatePrompt(
			new ScenarioEvents.ConditionAfterTargetConfirmed.Parameters(abilityState), abilityState);
	}

	protected override string DefaultTargetingHintText(State abilityState)
	{
		string conditonIconsText = string.Empty;
		foreach(ConditionModel conditionModel in Conditions)
		{
			conditonIconsText += Icons.HintText(Icons.GetCondition(conditionModel));
		}

		if(Target == Target.Self)
		{
			return $"Perform {conditonIconsText} self?";
		}

		return $"Select a target for {conditonIconsText}";
	}
}
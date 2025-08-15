using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class OtherAbility : Ability<OtherAbility.State>
{
	public class State : AbilityState
	{
	}

	private readonly Func<State, GDTask> _performAbility;

	public OtherAbility(Func<State, GDTask> performAbility,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvents.AbilityPerformed.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		_performAbility = performAbility;
	}

	protected override async GDTask Perform(State abilityState)
	{
		await _performAbility(abilityState);
	}
}
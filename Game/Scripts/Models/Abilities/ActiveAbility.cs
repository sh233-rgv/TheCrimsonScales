using System;
using System.Collections.Generic;
using Fractural.Tasks;

public abstract class ActiveAbilityState : AbilityState
{
	private Func<ActiveAbilityState, GDTask> _onDeactivate;

	public void SetOnDeactivate(Func<ActiveAbilityState, GDTask> onDeactivate)
	{
		_onDeactivate = onDeactivate;
	}

	public override async GDTask RemoveFromActive()
	{
		await base.RemoveFromActive();

		if(_onDeactivate != null)
		{
			await _onDeactivate(this);
		}
	}
}

public abstract class ActiveAbility<T> : Ability<T> where T : ActiveAbilityState, new()
{
	private Func<T, string> _getHintText;

	protected ActiveAbility(
		Func<T, GDTask> onAbilityStarted = null, Func<T, GDTask> onAbilityEnded = null, Func<T, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<T, string> getHintText = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		_getHintText = getHintText ?? DefaultHintText;
	}

	protected async GDTask AskConfirmAndActivate(T abilityState)
	{
		ConfirmPrompt.Answer confirmAnswer = await PromptManager.Prompt(new ConfirmPrompt(null, () => _getHintText(abilityState)), abilityState.Authority);
		if(confirmAnswer.Confirmed)
		{
			await Activate(abilityState);
		}
	}

	protected virtual async GDTask Activate(T abilityState)
	{
		abilityState.SetOnDeactivate(state => Deactivate((T)state));
		abilityState.SetPerformed();
		await abilityState.ActionState.SetPerformedActiveAbility(abilityState);
	}

	protected virtual async GDTask Deactivate(T abilityState)
	{
		await GDTask.CompletedTask;
	}

	protected virtual string DefaultHintText(T abilityState)
	{
		return "Perform the active ability?";
	}
}
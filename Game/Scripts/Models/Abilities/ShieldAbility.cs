using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class ShieldAbility : ActiveAbility<ShieldAbility.State>
{
	public class State : ActiveAbilityState
	{
	}

	private readonly Func<ScenarioEvents.SufferDamage.Parameters, bool> _customCanApply;
	private readonly bool _customCanApplyReplaceFully;

	public DynamicInt<State> ShieldValue { get; }
	public RangeType? RequiredRangeType { get; }
	public bool Pierceable { get; }

	public bool ConditionalValue => RequiredRangeType.HasValue || _customCanApply != null;

	public ShieldAbility(DynamicInt<State> shieldValue, RangeType? requiredRangeType = null, bool pierceable = true,
		Func<ScenarioEvents.SufferDamage.Parameters, bool> customCanApply = null, bool customCanApplyReplaceFully = false,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getHintText = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, getHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		ShieldValue = shieldValue;
		RequiredRangeType = requiredRangeType;
		Pierceable = pierceable;
		_customCanApply = customCanApply;
		_customCanApplyReplaceFully = customCanApplyReplaceFully;
	}

	protected override async GDTask Perform(State abilityState)
	{
		await AskConfirmAndActivate(abilityState);
	}

	protected override async GDTask Activate(State abilityState)
	{
		await base.Activate(abilityState);

		ScenarioCheckEvents.ShieldCheckEvent.Subscribe(abilityState, this,
			parameters =>
				parameters.Figure == abilityState.Performer,
			parameters =>
			{
				if(ConditionalValue)
				{
					parameters.SetExtraValue();
				}
				else
				{
					int shieldValue = ShieldValue.GetValue(abilityState);
					parameters.AdjustShield(shieldValue);
				}
			}
		);

		//abilityState.Performer.UpdateShield();

		ScenarioEvents.SufferDamageEvent.Subscribe(abilityState, this,
			parameters =>
			{
				bool canApply =
					parameters.Figure == abilityState.Performer && parameters.FromAttack &&
					(!RequiredRangeType.HasValue || parameters.PotentialAttackAbilityState.SingleTargetRangeType == RequiredRangeType);

				if(_customCanApply != null)
				{
					if(_customCanApplyReplaceFully)
					{
						return _customCanApply(parameters);
					}

					canApply = canApply && _customCanApply(parameters);
				}

				return canApply;
			},
			async parameters =>
			{
				int shieldValue = ShieldValue.GetValue(abilityState);

				if(Pierceable)
				{
					parameters.AdjustShield(shieldValue);
				}
				else
				{
					parameters.AdjustUnpierceableShield(shieldValue);
				}

				await GDTask.CompletedTask;
			}
		);

		AppController.Instance.AudioController.PlayFastForwardable(SFX.Shield, delay: 0f);
	}

	protected override async GDTask Deactivate(State abilityState)
	{
		await base.Deactivate(abilityState);

		ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(abilityState, this);

		//abilityState.Performer.UpdateShield();

		ScenarioEvents.SufferDamageEvent.Unsubscribe(abilityState, this);
	}

	protected override string DefaultHintText(State abilityState)
	{
		return $"Perform the {Icons.HintText(Icons.Shield)} ability?";
	}
}
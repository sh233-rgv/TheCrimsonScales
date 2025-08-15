using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class GrantAbility : TargetedAbility<GrantAbility.State, SingleTargetState>
{
	public class State : TargetedAbilityState<SingleTargetState>
	{
	}

	private readonly Func<Figure, List<Ability>> _getAbilities;

	public List<ScenarioEvents.DuringGrant.Subscription> DuringGrantSubscriptions { get; }

	public GrantAbility(Func<Figure, List<Ability>> getAbilities, int targets = 1, int? range = null, RangeType? rangeType = null,
		Target target = Target.Allies,
		bool requiresLineOfSight = true, bool mandatory = false,
		Hex targetHex = null,
		AOEPattern aoePattern = null, int push = 0, int pull = 0, ConditionModel[] conditions = null,
		Action<State, List<Figure>> customGetTargets = null,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getTargetingHintText = null,
		List<ScenarioEvents.DuringGrant.Subscription> duringGrantSubscriptions = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(targets, range, rangeType, target,
			requiresLineOfSight, mandatory, targetHex, aoePattern, push, pull, conditions,
			customGetTargets, onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed,
			conditionalAbilityCheck, getTargetingHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		_getAbilities = getAbilities;
		DuringGrantSubscriptions = duringGrantSubscriptions;
	}

	protected override async GDTask StartPerform(State abilityState)
	{
		await base.StartPerform(abilityState);

		ScenarioEvents.DuringGrantEvent.Subscribe(abilityState, this, DuringGrantSubscriptions);
	}

	protected override async GDTask EndPerform(State abilityState)
	{
		await base.EndPerform(abilityState);

		ScenarioEvents.DuringGrantEvent.Unsubscribe(DuringGrantSubscriptions);
	}

	protected override EffectCollection CreateDuringTargetedAbilityEffectCollection(State abilityState)
	{
		return ScenarioEvents.DuringGrantEvent.CreateEffectCollection(new ScenarioEvents.DuringGrant.Parameters(abilityState));
	}

	protected override async GDTask AfterTargetConfirmedBeforeConditionsApplied(State abilityState, Figure target)
	{
		await base.AfterTargetConfirmedBeforeConditionsApplied(abilityState, target);

		// Perform the actual abilities
		ActionState actionState = new ActionState(target, target is Character ? target : abilityState.Performer, _getAbilities(target), abilityState.ActionState);
		await actionState.Perform();
	}
}
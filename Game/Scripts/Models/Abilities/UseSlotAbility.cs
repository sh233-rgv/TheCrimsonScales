using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class UseSlotAbility : ActiveAbility<UseSlotAbility.State>
{
	public class State : ActiveAbilityState
	{
		public List<UseSlot> Slots { get; private set; }
		public int UseSlotIndex { get; private set; }

		public void SetSlots(List<UseSlot> slots)
		{
			Slots = slots;
		}

		public async GDTask AdvanceUseSlot()
		{
			UseSlot from = Slots[UseSlotIndex];

			if(from.OnExit != null)
			{
				await from.OnExit.Invoke(this);
			}

			UseSlotIndex++;

			if(UseSlotIndex >= Slots.Count)
			{
				await ActionState.RequestDiscardOrLose();
			}
		}

		public async GDTask MoveBackUseSlot()
		{
			UseSlotIndex--;

			await GDTask.CompletedTask;
		}
	}

	public List<UseSlot> UseSlots { get; }
	public Func<State, GDTask> OnActivate { get; }
	public Func<State, GDTask> OnDeactivate { get; }

	public UseSlotAbility(List<UseSlot> useSlots,
		Func<State, GDTask> onActivate, Func<State, GDTask> onDeactivate,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getHintText = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, getHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		UseSlots = useSlots;
		OnActivate = onActivate;
		OnDeactivate = onDeactivate;
	}

	protected override async GDTask Perform(State abilityState)
	{
		await AskConfirmAndActivate(abilityState);
	}

	protected override async GDTask Activate(State abilityState)
	{
		await base.Activate(abilityState);

		abilityState.SetSlots(UseSlots);

		await OnActivate(abilityState);
	}

	protected override async GDTask Deactivate(State abilityState)
	{
		await base.Deactivate(abilityState);

		await OnDeactivate(abilityState);
	}
}
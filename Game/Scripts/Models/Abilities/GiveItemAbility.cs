using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class GiveItemAbility : TargetedAbility<GiveItemAbility.State, SingleTargetState>
{
	public class State : TargetedAbilityState<SingleTargetState>
	{
	}

	private readonly Action<AbilityState, List<ItemModel>> _getItems;
	private readonly Func<AbilityState, ItemModel, GDTask> _onItemGiven;
	private readonly Func<ItemModel, GDTask> _onItemConsumed;

	private readonly bool _selectAutomatically;

	public GiveItemAbility(Action<AbilityState, List<ItemModel>> getItems,
		Func<AbilityState, ItemModel, GDTask> onItemGiven = null,
		Func<ItemModel, GDTask> onItemConsumed = null,
		bool selectAutomatically = false,
		int targets = 1, int? range = null, RangeType? rangeType = null,
		Target target = Target.Allies | Target.MustTargetCharacters,
		bool requiresLineOfSight = true, bool mandatory = false,
		Hex targetHex = null,
		AOEPattern aoePattern = null, int push = 0, int pull = 0, ConditionModel[] conditions = null,
		Action<State, List<Figure>> customGetTargets = null,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getTargetingHintText = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(targets, range, rangeType, target,
			requiresLineOfSight, mandatory, targetHex, aoePattern, push, pull, conditions,
			customGetTargets, onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed,
			conditionalAbilityCheck, getTargetingHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		_getItems = getItems;
		_onItemGiven = onItemGiven;
		_onItemConsumed = onItemConsumed;

		_selectAutomatically = selectAutomatically;
	}

	protected override async GDTask AfterTargetConfirmedBeforeConditionsApplied(State abilityState, Figure target)
	{
		await base.AfterTargetConfirmedBeforeConditionsApplied(abilityState, target);

		if(target is not Character character)
		{
			Log.Error("Trying to give an item to a figure that isn't a character.");
			return;
		}

		await GiveItem(abilityState, character, _getItems, _onItemGiven, _onItemConsumed, _selectAutomatically);
	}

	public static async GDTask GiveItem(AbilityState abilityState, Character target, Action<AbilityState, List<ItemModel>> getItems,
		Func<AbilityState, ItemModel, GDTask> onItemGiven, Func<ItemModel, GDTask> onItemConsumed, bool selectAutomatically = false)
	{
		ItemModel item;
		List<ItemModel> items = new List<ItemModel>();
		getItems(abilityState, items);

		if(selectAutomatically)
		{
			item = items.Count == 0 ? null : items[0];
		}
		else
		{
			item = await AbilityCmd.SelectItem(abilityState.Authority, items, "Select an item to give");
		}

		if(item != null && target is Character character)
		{
			if(onItemGiven != null)
			{
				await onItemGiven(abilityState, item);
			}

			character.AddItem(item);

			abilityState.SetPerformed();

			object subscriber = new object();
			ScenarioEvents.ItemStateChangedEvent.Subscribe(abilityState, subscriber,
				parameters => parameters.Item == item,
				async parameters =>
				{
					if(item.ItemState == ItemState.Consumed)
					{
						if(onItemConsumed != null)
						{
							ScenarioEvents.ItemStateChangedEvent.Unsubscribe(abilityState, subscriber);

							await onItemConsumed(item);
						}
					}
				}
			);
		}
	}
}
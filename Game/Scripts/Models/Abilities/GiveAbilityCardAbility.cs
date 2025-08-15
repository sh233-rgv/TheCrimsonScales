using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class GiveAbilityCardAbility : TargetedAbility<GiveAbilityCardAbility.State, SingleTargetState>
{
	public class State : TargetedAbilityState<SingleTargetState>
	{
	}

	private readonly Action<AbilityState, List<AbilityCard>> _getAbilityCards;
	private readonly Func<AbilityState, AbilityCard, GDTask> _onCardGiven;
	private readonly Func<AbilityCard, GDTask> _onCardDiscarded;
	private readonly Func<AbilityCard, GDTask> _onCardLost;

	private readonly bool _selectAutomatically;

	public GiveAbilityCardAbility(Action<AbilityState, List<AbilityCard>> getAbilityCards,
		Func<AbilityState, AbilityCard, GDTask> onCardGiven = null,
		Func<AbilityCard, GDTask> onCardDiscarded = null,
		Func<AbilityCard, GDTask> onCardLost = null,
		bool selectAutomatically = false,
		int targets = 1, int? range = null, RangeType? rangeType = null,
		Target target = Target.Allies,
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
		_getAbilityCards = getAbilityCards;
		_onCardGiven = onCardGiven;
		_onCardDiscarded = onCardDiscarded;
		_onCardLost = onCardLost;

		_selectAutomatically = selectAutomatically;
	}

	protected override async GDTask AfterTargetConfirmedBeforeConditionsApplied(State abilityState, Figure target)
	{
		await base.AfterTargetConfirmedBeforeConditionsApplied(abilityState, target);

		await GiveAbilityCard(abilityState, target, _getAbilityCards, _onCardGiven, _onCardDiscarded, _onCardLost, _selectAutomatically);
	}

	public static async GDTask GiveAbilityCard(AbilityState abilityState, Figure target, Action<AbilityState, List<AbilityCard>> getAbilityCards,
		Func<AbilityState, AbilityCard, GDTask> onCardGiven, Func<AbilityCard, GDTask> onCardDiscarded, Func<AbilityCard, GDTask> onCardLost, bool selectAutomatically = false)
	{
		AbilityCard abilityCard;
		if(selectAutomatically)
		{
			List<AbilityCard> abilityCards = new List<AbilityCard>();
			getAbilityCards(abilityState, abilityCards);
			abilityCard = abilityCards.Count == 0 ? null : abilityCards[0];
		}
		else
		{
			abilityCard = await AbilityCmd.SelectAbilityCard(abilityState.Authority, list => getAbilityCards(abilityState, list), CardState.Hand);
		}

		if(abilityCard != null && target is Character character)
		{
			if(onCardGiven != null)
			{
				await onCardGiven(abilityState, abilityCard);
			}

			character.AddCard(abilityCard);

			object subscriber = new object();
			ScenarioEvents.AbilityCardStateChangedEvent.Subscribe(abilityState, subscriber,
				parameters => parameters.AbilityCard == abilityCard,
				async parameters =>
				{
					if(abilityCard.CardState == CardState.Discarded)
					{
						if(onCardDiscarded != null)
						{
							ScenarioEvents.AbilityCardStateChangedEvent.Unsubscribe(abilityState, subscriber);

							await onCardDiscarded(abilityCard);
						}
					}
					else if(abilityCard.CardState == CardState.Lost)
					{
						if(onCardLost != null)
						{
							ScenarioEvents.AbilityCardStateChangedEvent.Unsubscribe(abilityState, subscriber);

							await onCardLost(abilityCard);
						}
					}
				}
			);
		}
	}
}
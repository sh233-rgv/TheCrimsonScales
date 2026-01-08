using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public abstract class AbilityCardSideModel : AbstractModel
{
	// private List<EnhancementMark> _enhancements;
	private IEnumerable<AbilityCardAbility> _abilities;

	protected virtual IEnumerable<Element> Elements { get; } = [];
	protected virtual int XP => 0;

	protected virtual bool Round => false;
	protected virtual bool Persistent => false;
	public virtual bool Loss => false;
	protected virtual bool Unrecoverable => false;
	protected virtual bool CanDeactivate => true;

	// public List<EnhancementMark> Enhancements
	// {
	// 	get
	// 	{
	// 		if(_enhancements == null)
	// 		{
	// 			_enhancements = GetEnhancements();
	// 		}
	//
	// 		return _enhancements;
	// 	}
	// }

	public IEnumerable<AbilityCardAbility> Abilities
	{
		get
		{
			if(_abilities == null)
			{
				_abilities = GetAbilities();
			}

			return _abilities;
		}
	}

	// protected virtual List<EnhancementMark> GetEnhancements() => [];
	protected abstract List<AbilityCardAbility> GetAbilities();

	public bool GetIsTop(AbilityCard abilityCard) => abilityCard.Top == this;
	public bool GetIsBasicTop(AbilityCard abilityCard) => abilityCard.BasicTop == this;
	public bool GetIsBottom(AbilityCard abilityCard) => abilityCard.Bottom == this;
	public bool GetIsBasicBottom(AbilityCard abilityCard) => abilityCard.BasicBottom == this;

	public async GDTask Perform(Figure performer, AbilityCard abilityCard)
	{
		ScenarioEvents.AbilityCardSideStarted.Parameters startedParameters =
			await ScenarioEvents.AbilityCardSideStartedEvent.CreatePrompt(
				new ScenarioEvents.AbilityCardSideStarted.Parameters(this, performer));

		CardState resultingState = CardState.Discarded;

		if(!startedParameters.ForgoneAction)
		{
			ActionState actionState = new ActionState(abilityCard, performer, Abilities.Select(ability => ability.Ability).ToList(), //null, 
				onFirstActivateAbilityActivated: OnFirstActivateAbilityActivated, onDiscardOrLoseRequested: OnDiscardOrLoseRequested);
			await actionState.Perform();

			if(actionState.GetHasPerformed())
			{
				await OnActionPerformed(actionState.Performer);

				await AbilityCmd.GainXP(performer, XP);

				foreach(Element element in Elements)
				{
					await AbilityCmd.InfuseElement(null, element, performer);
				}

				bool round = Round || actionState.OverrideRound;
				bool persistent = !actionState.OverrideNoPersistent && (actionState.OverridePersistent || Persistent);
				bool loss = !actionState.OverrideNoLoss && (actionState.OverrideLoss || Loss);

				if(round && persistent)
				{
					Log.Error($"Ability card side {this} is supposed to be both only active for the round, and persistent. This is not allowed.");
				}

				abilityCard.SetUnrecoverable(Unrecoverable);

				// If no persistent/round ability has been performed, discard or lose it instead
				if(actionState.HasPerformedActiveAbility && !actionState.OverrideNoPersistent)
				{
					if(round)
					{
						resultingState = loss ? CardState.RoundLoss : CardState.Round;
					}
					else if(persistent)
					{
						resultingState = loss ? CardState.PersistentLoss : (CanDeactivate ? CardState.Persistent : CardState.PersistentNoDeactivate);
					}
					else
					{
						Log.Error($"Ability card side {this} performed an active ability, but is not marked as a round or persistent card.");
					}
				}
				else
				{
					if(loss)
					{
						resultingState = Unrecoverable ? CardState.UnrecoverablyLost : CardState.Lost;
					}
				}

				await abilityCard.SetCardState(resultingState);
			}
			else
			{
				await AbilityCmd.DiscardCard(abilityCard);
			}
		}
		else
		{
			await AbilityCmd.DiscardCard(abilityCard);
		}

		await ScenarioEvents.AbilityCardSideEndedEvent.CreatePrompt(
			new ScenarioEvents.AbilityCardSideEnded.Parameters(this, performer, resultingState));
	}

	protected async GDTask GainXP(AbilityState abilityState)
	{
		await AbilityCmd.GainXP(abilityState.Performer, 1);
	}

	private async GDTask OnFirstActivateAbilityActivated(ActionState actionState)
	{
		((AbilityCard)actionState.ActionSource).SetActionStateActive(actionState);

		await GDTask.CompletedTask;
	}

	private async GDTask OnDiscardOrLoseRequested(ActionState actionState)
	{
		await AbilityCmd.DiscardOrLose(((AbilityCard)actionState.ActionSource));
	}

	protected virtual async GDTask OnActionPerformed(Figure figure)
	{
		await GDTask.CompletedTask;
	}
}
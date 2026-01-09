using System.Collections.Generic;
using Fractural.Tasks;

public abstract class AbilityCardSideModel<TCharacter> : AbilityCardSideModel
	where TCharacter : Character
{
	protected TCharacter GetOriginalOwner(AbilityState abilityState)
	{
		return (TCharacter)GetAbilityCardSide(abilityState).AbilityCard.OriginalOwner;
	}
}

public abstract class AbilityCardSideModel : AbstractModel
{
	// private List<EnhancementMark> _enhancements;
	private IEnumerable<AbilityCardAbility> _abilities;

	public AbilityCardModel AbilityCardModel { get; private set; }
	public virtual AbilityCardSideType AbilityCardSideType { get; private set; }

	public virtual IEnumerable<Element> Elements { get; } = [];
	public virtual int XP => 0;

	public virtual bool Round => false;
	public virtual bool Persistent => false;
	public virtual bool Loss => false;
	public virtual bool Unrecoverable => false;
	public virtual bool CanDeactivate => true;

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

	public void Init(AbilityCardModel abilityCardModel, AbilityCardSideType abilityCardSideType)
	{
		AbilityCardModel = abilityCardModel;
		AbilityCardSideType = abilityCardSideType;
	}

	// protected virtual List<EnhancementMark> GetEnhancements() => [];
	protected abstract List<AbilityCardAbility> GetAbilities();

	protected AbilityCardSide GetAbilityCardSide(AbilityState abilityState)
	{
		return (AbilityCardSide)abilityState.ActionState.ActionSource;
	}

	protected AbilityCard GetAbilityCard(AbilityState abilityState)
	{
		return GetAbilityCardSide(abilityState).AbilityCard;
	}

	protected async GDTask GainXP(AbilityState abilityState)
	{
		await AbilityCmd.GainXP(abilityState.Performer, 1);
	}

	public virtual async GDTask OnActionPerformed(Figure figure)
	{
		await GDTask.CompletedTask;
	}
}
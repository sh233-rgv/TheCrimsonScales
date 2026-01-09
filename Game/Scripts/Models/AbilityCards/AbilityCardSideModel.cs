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
	private IEnumerable<AbilityCardAbility> _abilities;
	private readonly List<EnhancementMark> _enhancements = new List<EnhancementMark>();

	public AbilityCardModel AbilityCardModel { get; private set; }
	public virtual AbilityCardSideType AbilityCardSideType { get; private set; }

	public virtual IEnumerable<Element> Elements { get; } = [];
	public virtual int XP => 0;

	public virtual bool Round => false;
	public virtual bool Persistent => false;
	public virtual bool Loss => false;
	public virtual bool Unrecoverable => false;
	public virtual bool CanDeactivate => true;

	public IEnumerable<AbilityCardAbility> Abilities
	{
		get
		{
			TryGetAbilities();

			return _abilities;
		}
	}

	public List<EnhancementMark> Enhancements
	{
		get
		{
			TryGetAbilities();

			return _enhancements;
		}
	}

	public void Init(AbilityCardModel abilityCardModel, AbilityCardSideType abilityCardSideType)
	{
		AbilityCardModel = abilityCardModel;
		AbilityCardSideType = abilityCardSideType;
	}

	public void RegisterEnhancementMark(EnhancementMark enhancementMark)
	{
		_enhancements.Add(enhancementMark);
	}

	public virtual async GDTask OnActionPerformed(Figure figure)
	{
		await GDTask.CompletedTask;
	}

	// protected virtual List<EnhancementMark> GetEnhancements() => [];
	protected abstract List<AbilityCardAbility> GetAbilities();

	// protected EnhancementMark<TPip> EnhancementMark<TPip>(TPip enhancementPipModel, Vector2 normalizedPosition)
	// 	where TPip : EnhancementPipModel
	// {
	// 	EnhancementMark<TPip> newMark = new EnhancementMark<TPip>(enhancementPipModel, normalizedPosition);
	// 	_enhancements.Add(newMark);
	//
	// 	return newMark;
	// }

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

	private void TryGetAbilities()
	{
		if(_abilities == null)
		{
			_abilities = GetAbilities();
		}
	}
}
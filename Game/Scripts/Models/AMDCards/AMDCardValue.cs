using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class AMDCardValue(
	bool rolling, AMDCardType cardType, int? value, int? pierce, int? push, int? pull, int? swing,
	List<Element> elements, List<ConditionModel> conditionModels, List<Ability> abilities, Func<AttackAbility.State, GDTask> extraEffects)
{
	public bool Rolling { get; } = rolling;

	public AMDCardType CardType { get; } = cardType;
	public int? Value { get; } = value;

	public int? Pierce { get; } = pierce;
	public int? Push { get; } = push;
	public int? Pull { get; } = pull;
	public int? Swing { get; } = swing;
	public List<Element> Elements { get; } = elements;
	public List<ConditionModel> ConditionModels { get; } = conditionModels;
	public List<Ability> Abilities { get; } = abilities;
	public Func<AttackAbility.State, GDTask> ExtraEffects { get; } = extraEffects;

	public async GDTask Apply(AttackAbility.State attackAbilityState)
	{
		ScenarioEvents.AMDCardValueApplied.Parameters amdCardValueAppliedParameters =
			await ScenarioEvents.AMDCardValueAppliedEvent.CreatePrompt(
				new ScenarioEvents.AMDCardValueApplied.Parameters(attackAbilityState, this), attackAbilityState);

		int adjustedValue = amdCardValueAppliedParameters.AMDCardValue.GetAttackModifierValue(attackAbilityState);
		attackAbilityState.SingleTargetAdjustAttackValue(adjustedValue);

		if(Pierce.HasValue)
		{
			attackAbilityState.SingleTargetAdjustPierce(Pierce.Value);
		}

		if(Push.HasValue)
		{
			attackAbilityState.SingleTargetAdjustPush(Push.Value);
		}

		if(Pull.HasValue)
		{
			attackAbilityState.SingleTargetAdjustPull(Pull.Value);
		}

		if(Swing.HasValue)
		{
			attackAbilityState.SingleTargetAdjustSwing(Swing.Value);
		}

		foreach(Element element in Elements)
		{
			await AbilityCmd.InfuseElement(element);
		}

		foreach(ConditionModel condition in ConditionModels)
		{
			attackAbilityState.SingleTargetAddCondition(condition);
		}

		if(Abilities.Count > 0)
		{
			ScenarioEvents.AfterAttackPerformedEvent.Subscribe(attackAbilityState, this,
				parameters =>
					attackAbilityState == parameters.AbilityState &&
					parameters.AbilityState.Target == attackAbilityState.Target,
				async parameters =>
				{
					ScenarioEvents.AfterAttackPerformedEvent.Unsubscribe(attackAbilityState, this);

					ActionState actionState = new ActionState(attackAbilityState.Performer, Abilities);
					await actionState.Perform();
				}
			);
		}

		if(ExtraEffects != null)
		{
			await ExtraEffects.Invoke(attackAbilityState);
		}
	}

	public int GetAttackModifierValue(AttackAbility.State attackAbilityState)
	{
		int attackModifierValue = 0;
		if(CardType == AMDCardType.Crit)
		{
			attackModifierValue = attackAbilityState.SingleTargetAttackValue;
		}
		else if(CardType == AMDCardType.Null)
		{
			attackModifierValue = -attackAbilityState.SingleTargetAttackValue;
		}
		else if(CardType == AMDCardType.Value && Value.HasValue)
		{
			attackModifierValue = Value.Value;
		}

		return attackModifierValue;
	}

	public bool GetHasExtraEffects(AttackAbility.State attackAbilityState)
	{
		return Pierce.HasValue || Push.HasValue || Pull.HasValue || Swing.HasValue ||
		       Elements.Count > 0 || ConditionModels.Count > 0 || Abilities.Count > 0 || ExtraEffects != null;
	}
}
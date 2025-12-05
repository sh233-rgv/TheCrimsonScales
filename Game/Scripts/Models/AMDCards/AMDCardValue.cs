using System;
using System.Collections.Generic;
using Fractural.Tasks;

public class AMDCardValue(
	bool rolling, AMDCardType cardType, int? value, int? pierce, int? push, int? pull, int? swing,
	List<Element> elements, List<ConditionModel> conditionModels, Func<AttackAbility.State, GDTask> extraEffects)
{
	public bool Rolling { get; private set; } = rolling;

	public AMDCardType CardType { get; private set; } = cardType;
	public int? Value { get; private set; } = value;

	public int? Pierce { get; private set; } = pierce;
	public int? Push { get; private set; } = push;
	public int? Pull { get; private set; } = pull;
	public int? Swing { get; private set; } = swing;
	public List<Element> Elements { get; private set; } = elements;
	public List<ConditionModel> ConditionModels { get; private set; } = conditionModels;
	public Func<AttackAbility.State, GDTask> ExtraEffects { get; private set; } = extraEffects;

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

		if(ExtraEffects != null)
		{
			ExtraEffects?.Invoke(attackAbilityState);
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
		       Elements.Count > 0 || ConditionModels.Count > 0 || extraEffects != null;
	}
}
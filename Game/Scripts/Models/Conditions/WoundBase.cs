using Fractural.Tasks;

public abstract class WoundBase : ConditionModel
{
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Wound.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Negative;
	public override bool RemovedByHeal => true;
	public virtual bool CanBeUpgraded => true;
	public override ConditionModel[] ImmunityCompareBaseConditions => [Conditions.Wound1];

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		for(int i = target.Conditions.Count - 1; i >= 0; i--)
		{
			ConditionModel condition = target.Conditions[i];
			if(condition is WoundBase && condition != this)
			{
				await AbilityCmd.RemoveCondition(target, condition);
			}
		}

		Node.SetStackText(WoundValue == 1 ? null : WoundValue.ToString());

		ScenarioEvents.FigureTurnStartedEvent.Subscribe(condition,
			parameters => parameters.Figure == condition.Owner,
			async parameters =>
			{
				condition.Flash();
				await AbilityCmd.SufferDamage(condition.Owner, UpgradableLevel, condition.Owner);
			},
			EffectType.MandatoryBeforeOptionals
		);

		// ScenarioEvents.AfterHealPerformedEvent.Subscribe(this,
		// 	parameters => parameters.AbilityState.Target == Owner,
		// 	async parameters => await AbilityCmd.RemoveCondition(Owner, this), EffectType.MandatoryBeforeOptionals);
	}

	public override async GDTask OnRemoved(Condition condition)
	{
		await base.OnRemoved(condition);

		ScenarioEvents.FigureTurnStartedEvent.Unsubscribe(condition);
		//ScenarioEvents.AfterHealPerformedEvent.Unsubscribe(this);
	}

	protected override bool DuplicatesCheckCanApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		return
			base.DuplicatesCheckCanApply(parameters) ||
			(!parameters.Prevented && parameters.Target == Owner && parameters.ConditionModel.ImmutableInstance is WoundBase woundBase &&
			 woundBase.WoundValue < WoundValue);
	}
}
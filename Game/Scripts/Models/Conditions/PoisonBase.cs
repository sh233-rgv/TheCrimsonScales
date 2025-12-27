using Fractural.Tasks;

public abstract class PoisonBase : ConditionModel
{
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Poison.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Negative;
	public override bool RemovedByHeal => true;
	public virtual bool CanBeUpgraded => true;
	public override ConditionModel[] ImmunityCompareBaseConditions => [Conditions.Poison1];

	protected abstract int PoisonValue { get; }

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		for(int i = target.Conditions.Count - 1; i >= 0; i--)
		{
			ConditionModel condition = target.Conditions[i];
			if(condition is PoisonBase && condition != this)
			{
				await AbilityCmd.RemoveCondition(target, condition);
			}
		}

		Node.SetStackText(PoisonValue == 1 ? null : PoisonValue.ToString());

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(this, CanApply, Apply, EffectType.MandatoryBeforeOptionals);

		ScenarioEvents.HealBlockTimeEvent.Subscribe(this,
			parameters => parameters.AbilityState.Target == Owner,
			parameters =>
			{
				Node.Flash();
				parameters.SetBlocked(true);
				return GDTask.CompletedTask;
			},
			EffectType.MandatoryBeforeOptionals
		);

		// ScenarioEvents.AfterHealPerformedEvent.Subscribe(this,
		// 	parameters => parameters.AbilityState.Target == Owner,
		// 	async parameters =>
		// 	{
		// 		await AbilityCmd.RemoveCondition(Owner, this);
		// 	},
		// 	EffectType.MandatoryBeforeOptionals
		// );
	}

	public override async GDTask OnRemoved(Condition condition)
	{
		await base.OnRemoved(condition);

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(this);
		ScenarioEvents.HealBlockTimeEvent.Unsubscribe(this);
		//ScenarioEvents.AfterHealPerformedEvent.Unsubscribe(this);
	}

	protected override bool DuplicatesCheckCanApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		return
			base.DuplicatesCheckCanApply(parameters) ||
			(!parameters.Prevented && parameters.Target == Owner && parameters.ConditionModel.ImmutableInstance is PoisonBase poisonBase &&
			 poisonBase.PoisonValue < PoisonValue);
	}

	private bool CanApply(ScenarioEvents.AttackAfterTargetConfirmed.Parameters abilityStateParameters)
	{
		return abilityStateParameters.AbilityState.Target == Owner;
	}

	private GDTask Apply(ScenarioEvents.AttackAfterTargetConfirmed.Parameters abilityStateParameters)
	{
		Node.Flash();
		abilityStateParameters.AbilityState.SingleTargetAdjustAttackValue(PoisonValue);
		return GDTask.CompletedTask;
	}
}
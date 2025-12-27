using Fractural.Tasks;

public class Infect : ConditionModel
{
	public override string Name => "Infect";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Infect.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Negative;
	public override bool RemovedByHeal => true;
	public override ConditionModel[] ImmunityCompareBaseConditions => [Conditions.Poison1];

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		Owner.SetCrackedShield(true);

		ScenarioEvents.AbilityStartedEvent.Subscribe(this,
			parameters => parameters.Performer == Owner && parameters.AbilityState is ShieldAbility.State,
			parameters =>
			{
				Node.Flash();
				parameters.SetIsBlocked(true);
				return GDTask.CompletedTask;
			},
			EffectType.MandatoryBeforeOptionals);
		ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(this, CanApply, Apply, EffectType.MandatoryBeforeOptionals);
	}

	public override async GDTask OnRemoved(Condition condition)
	{
		await base.OnRemoved(condition);

		Owner.SetCrackedShield(false);

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(this);
		ScenarioEvents.AbilityStartedEvent.Unsubscribe(this);
	}

	private bool CanApply(ScenarioEvents.AttackAfterTargetConfirmed.Parameters abilityStateParameters)
	{
		return abilityStateParameters.AbilityState.Target == Owner;
	}

	private GDTask Apply(ScenarioEvents.AttackAfterTargetConfirmed.Parameters abilityStateParameters)
	{
		Node.Flash();
		abilityStateParameters.AbilityState.SingleTargetSetIgnoresAllShields();
		return GDTask.CompletedTask;
	}
}
using Fractural.Tasks;

public class Muddle : ConditionModel
{
	public override string Name => "Muddle";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Muddle.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Negative;
	public override bool RemovedAtEndOfTurn => true;

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(this,
			parameters => parameters.Performer == Owner,
			parameters =>
			{
				Node.Flash();
				parameters.AbilityState.SingleTargetSetHasDisadvantage();
				return GDTask.CompletedTask;
			},
			EffectType.MandatoryBeforeOptionals);

		ScenarioCheckEvents.DisadvantageCheckEvent.Subscribe(this,
			canApplyParameters => canApplyParameters.Attacker == Owner,
			applyParameters => applyParameters.SetDisadvantage(true));
	}

	public override async GDTask OnRemoved(Condition condition)
	{
		await base.OnRemoved(condition);

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(this);
		ScenarioCheckEvents.DisadvantageCheckEvent.Unsubscribe(this);
	}
}
using Fractural.Tasks;

public class Stun : ConditionModel
{
	public override string Name => "Stun";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Stun.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Negative;
	public override bool RemovedAtEndOfTurn => true;

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		ScenarioEvents.AbilityStartedEvent.Subscribe(Owner, this,
			parameters => parameters.Performer == Owner && !parameters.AbilityState.CanPerformWhileStunned,
			parameters =>
			{
				Node.Flash();
				parameters.SetIsBlocked(true);

				return GDTask.CompletedTask;
			},
			EffectType.MandatoryBeforeOptionals);

		ScenarioEvents.CanMoveFurtherCheckEvent.Subscribe(Owner, this,
			parameters => parameters.Performer == Owner,
			parameters =>
			{
				Node.Flash();
				parameters.SetCannotMoveFurther(true);

				return GDTask.CompletedTask;
			}
			, order: 100
		);
	}

	public override async GDTask OnRemoved(Condition condition)
	{
		await base.OnRemoved(condition);

		ScenarioEvents.AbilityStartedEvent.Unsubscribe(Owner, this);
		ScenarioEvents.CanMoveFurtherCheckEvent.Unsubscribe(Owner, this);
	}
}
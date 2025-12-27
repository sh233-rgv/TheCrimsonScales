using Fractural.Tasks;

public class Disarm : ConditionModel
{
	public override string Name => "Disarm";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Disarm.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Negative;
	public override bool RemovedAtEndOfTurn => true;

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		ScenarioEvents.AbilityStartedEvent.Subscribe(this,
			parameters => parameters.Performer == Owner && parameters.AbilityState is AttackAbility.State,
			parameters =>
			{
				Node.Flash();
				parameters.SetIsBlocked(true);
				return GDTask.CompletedTask;
			},
			EffectType.MandatoryBeforeOptionals);
	}

	public override async GDTask OnRemoved(Condition condition)
	{
		await base.OnRemoved(TODO);

		ScenarioEvents.AbilityStartedEvent.Unsubscribe(this);
	}
}
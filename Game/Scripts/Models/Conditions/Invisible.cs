using Fractural.Tasks;

public class Invisible : ConditionModel
{
	public override string Name => "Invisible";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Invisible.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Positive;
	public override bool RemovedAtEndOfTurn => true;

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		ScenarioCheckEvents.CanBeFocusedCheckEvent.Subscribe(Owner, this,
			parameters => parameters.PotentialTarget == Owner && parameters.Performer.EnemiesWith(Owner),
			parameters =>
			{
				parameters.SetCannotBeFocused();
			}
		);

		ScenarioCheckEvents.CanBeTargetedCheckEvent.Subscribe(Owner, this,
			parameters => parameters.PotentialTarget == Owner && parameters.Performer.EnemiesWith(Owner),
			parameters =>
			{
				parameters.SetCannotBeTargeted();
			}
		);

		ScenarioCheckEvents.CanPassEnemyCheckEvent.Subscribe(Owner, this,
			parameters => parameters.EnemyFigure == Owner,
			parameters =>
			{
				parameters.SetCanPass();
			}
		);
	}

	public override async GDTask OnRemoved(Condition condition)
	{
		await base.OnRemoved(condition);

		ScenarioCheckEvents.CanBeFocusedCheckEvent.Unsubscribe(Owner, this);
		ScenarioCheckEvents.CanBeTargetedCheckEvent.Unsubscribe(Owner, this);
		ScenarioCheckEvents.CanPassEnemyCheckEvent.Unsubscribe(Owner, this);
	}
}
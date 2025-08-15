using Fractural.Tasks;

public class Invisible : ConditionModel
{
	public override string Name => "Invisible";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Invisible.svg";
	public override bool RemovedAtEndOfTurn => true;
	public override bool IsPositive => true;

	public override async GDTask Add(Figure target, ConditionNode node)
	{
		await base.Add(target, node);

		ScenarioCheckEvents.CanBeFocusedCheckEvent.Subscribe(this,
			parameters => parameters.PotentialTarget == Owner && parameters.Performer.EnemiesWith(Owner),
			parameters =>
			{
				parameters.SetCannotBeFocused();
			}
		);

		ScenarioCheckEvents.CanBeTargetedCheckEvent.Subscribe(this,
			parameters => parameters.PotentialTarget == Owner && parameters.Performer.EnemiesWith(Owner),
			parameters =>
			{
				parameters.SetCannotBeTargeted();
			}
		);
	}

	public override async GDTask Remove()
	{
		await base.Remove();

		ScenarioCheckEvents.CanBeFocusedCheckEvent.Unsubscribe(this);
		ScenarioCheckEvents.CanBeTargetedCheckEvent.Unsubscribe(this);
	}
}
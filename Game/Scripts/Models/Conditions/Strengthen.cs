using Fractural.Tasks;

public class Strengthen : ConditionModel
{
	public override string Name => "Strengthen";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Strengthen.svg";
	public override bool RemovedAtEndOfTurn => true;
	public override bool IsPositive => true;

	public override async GDTask Add(Figure target, ConditionNode node)
	{
		await base.Add(target, node);

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(this,
			parameters => parameters.Performer == Owner,
			parameters =>
			{
				Node.Flash();
				parameters.AbilityState.SingleTargetSetHasAdvantage();
				return GDTask.CompletedTask;
			},
			EffectType.MandatoryBeforeOptionals);
	}

	public override async GDTask Remove()
	{
		await base.Remove();

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(this);
	}
}
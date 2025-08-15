using Fractural.Tasks;

public class Ward : ConditionModel
{
	public override string Name => "Ward";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Ward.svg";
	public override bool IsPositive => true;

	public override async GDTask Add(Figure target, ConditionNode node)
	{
		await base.Add(target, node);

		ScenarioEvents.SufferDamageEvent.Subscribe(this,
			parameters => parameters.Figure == Owner,
			async parameters =>
			{
				Node.Flash();
				parameters.SetWard(true);

				await AbilityCmd.RemoveCondition(target, this);
			},
			EffectType.MandatoryBeforeOptionals, 100);
	}

	public override async GDTask Remove()
	{
		await base.Remove();

		ScenarioEvents.SufferDamageEvent.Unsubscribe(this);
	}
}
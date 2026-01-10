using Fractural.Tasks;

public class ShieldTrait(int shield) : FigureTrait
{
	private int _shield = shield;
	public override async GDTask Activate(Figure figure)
	{
		await base.Activate(figure);

		ScenarioCheckEvents.ShieldCheckEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure,
			applyParameters => { applyParameters.AdjustShield(_shield); });

		ScenarioEvents.SufferDamageEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure && canApplyParameters.FromAttack,
			async applyParameters =>
			{
				applyParameters.AdjustShield(_shield);
				await GDTask.CompletedTask;
			});

		//figure.UpdateShield();
	}

	public void ChangeShieldValue(Figure figure, int change)
    {
	    _shield += change;
        ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.SufferDamageEvent.Unsubscribe(figure, this);

		ScenarioCheckEvents.ShieldCheckEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure,
			applyParameters => { applyParameters.AdjustShield(_shield); });

		ScenarioEvents.SufferDamageEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure && canApplyParameters.FromAttack,
			async applyParameters =>
			{
				applyParameters.AdjustShield(_shield);
				await GDTask.CompletedTask;
			});
    }

	public override async GDTask Deactivate(Figure figure)
	{
		await base.Deactivate(figure);

		ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.SufferDamageEvent.Unsubscribe(figure, this);
	}
}
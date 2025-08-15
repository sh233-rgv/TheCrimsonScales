using Fractural.Tasks;

public class RetaliateTrait(int retaliate, int range = 1) : FigureTrait
{
	public override void Activate(Figure figure)
	{
		base.Activate(figure);

		ScenarioCheckEvents.RetaliateCheckEvent.Subscribe(figure, this,
			canApplyParameters =>
				canApplyParameters.Figure == figure,
			applyParameters =>
			{
				applyParameters.AddRetaliate(retaliate, range);
			});

		ScenarioEvents.RetaliateEvent.Subscribe(figure, this,
			canApplyParameters =>
			{
				return
					canApplyParameters.RetaliatingFigure == figure &&
					RangeHelper.Distance(canApplyParameters.AbilityState.Performer.Hex, figure.Hex) <= range;
			},
			async applyParameters =>
			{
				applyParameters.AdjustRetaliate(retaliate);
				await GDTask.CompletedTask;
			});

		//figure.UpdateRetaliate();
	}

	public override void Deactivate(Figure figure)
	{
		base.Deactivate(figure);

		ScenarioCheckEvents.RetaliateCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.RetaliateEvent.Unsubscribe(figure, this);

		//figure.UpdateRetaliate();
	}
}
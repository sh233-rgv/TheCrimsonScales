using Fractural.Tasks;

public class PermanentConditionTrait(ConditionModel condition) : FigureTrait
{
	public override async GDTask Activate(Figure figure)
	{
		await base.Activate(figure);

		await AbilityCmd.AddCondition(null, figure, condition);

		ScenarioEvents.AfterRemoveConditionEvent.Subscribe(figure, this,
			parameters => parameters.Figure == figure && parameters.Condition == condition,
			async parameters =>
			{
				await AbilityCmd.AddCondition(null, figure, condition);
			});

		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(figure, this,
			parameters => parameters.Figure == figure,
			parameters =>
			{
				parameters.Add(new InfoTextExtraEffect.Parameters(
					$"This figure always has {Icons.Inline(Icons.GetCondition(condition))}"));
			}
		);
	}

	public override async GDTask Deactivate(Figure figure)
	{
		await base.Deactivate(figure);

		ScenarioEvents.AfterRemoveConditionEvent.Unsubscribe(figure, this);
		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Unsubscribe(figure, this);
	}
}
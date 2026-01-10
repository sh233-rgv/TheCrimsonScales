using Fractural.Tasks;

public class HalfElementsShieldRetaliateTrait : FigureTrait
{
	public override async GDTask Activate(Figure figure)
	{
		await base.Activate(figure);

		ScenarioCheckEvents.ShieldCheckEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure,
			applyParameters =>
			{
				applyParameters.AdjustShield(CalculateElements());
			});

		ScenarioEvents.SufferDamageEvent.Subscribe(figure, this,
			canApplyParameters => canApplyParameters.Figure == figure && canApplyParameters.FromAttack,
			async applyParameters =>
			{
				applyParameters.AdjustShield(CalculateElements());
				await GDTask.CompletedTask;
			});
		
		ScenarioCheckEvents.RetaliateCheckEvent.Subscribe(figure, this,
			canApplyParameters =>
				canApplyParameters.Figure == figure,
			applyParameters =>
			{
				applyParameters.AddRetaliate(CalculateElements(), 1);
			});

		ScenarioEvents.RetaliateEvent.Subscribe(figure, this,
			canApplyParameters =>
			{
				return
					canApplyParameters.RetaliatingFigure == figure &&
					RangeHelper.Distance(canApplyParameters.AbilityState.Performer.Hex, figure.Hex) <= 1;
			},
			async applyParameters =>
			{
				applyParameters.AdjustRetaliate(CalculateElements());
				await GDTask.CompletedTask;
			});
		
		ScenarioEvents.ConsumeElementElement.Subscribe(figure, this,
			canApplyParameters => true,
			async applyParameters =>
			{
				ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();
				ScenarioCheckEvents.RetaliateCheckEvent.FireChangedEvent();
				await GDTask.CompletedTask;
			});
		
		//Fire change on Infuse
	}

	public override async GDTask Deactivate(Figure figure)
	{
		await base.Deactivate(figure);

		ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.SufferDamageEvent.Unsubscribe(figure, this);

		ScenarioCheckEvents.RetaliateCheckEvent.Unsubscribe(figure, this);
		ScenarioEvents.RetaliateEvent.Unsubscribe(figure, this);

		ScenarioEvents.ConsumeElementElement.Unsubscribe(figure, this);
	}

	private int CalculateElements()
	{
		int count = 0;
		for(int i = 0; i < 6; i++)
		{
			if(GameController.Instance.ElementManager.GetState((Element)i) is ElementState.Strong or ElementState.Waning)
			{
				count++;
			}
		}

		return (count + 1)/2;
	}
}
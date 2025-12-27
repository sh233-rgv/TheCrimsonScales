using Fractural.Tasks;
using Godot;

public abstract class ConditionModel : AbstractModel
{
	public abstract string Name { get; }
	public abstract string IconPath { get; }
	public abstract ConditionPolarity ConditionPolarity { get; }
	public virtual bool CanBeAppliedMultipleTimesInSingleTarget => false;
	public virtual ConditionModel[] ImmunityCompareBaseConditions => [this];
	public virtual bool RemovedAtEndOfTurn => false;
	public virtual bool ImmediatelyRemovedOnApply => false;
	public virtual bool RemovedByHeal => false;
	public virtual bool ShouldShowOnFigure => true;
	protected virtual string ConditionAnimationScenePath => null;

	public bool IsPositive => ConditionPolarity == ConditionPolarity.Positive;
	public bool IsNegative => ConditionPolarity == ConditionPolarity.Negative;

	public virtual async GDTask OnAdded(Condition condition)
	{
		ScenarioEvents.InflictConditionDuplicatesCheckEvent.Subscribe(this, DuplicatesCheckCanApply, DuplicatesCheckApply);

		if(RemovedAtEndOfTurn)
		{
			ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Subscribe(this, TurnEndedCanApply, TurnEndedApply);
		}

		if(!GameController.FastForward && ConditionAnimationScenePath != null)
		{
			PackedScene conditionScene = ResourceLoader.Load<PackedScene>(ConditionAnimationScenePath);
			ConditionAnimation conditionAnimation = conditionScene.Instantiate<ConditionAnimation>();
			GameController.Instance.Map.AddChild(conditionAnimation);
			conditionAnimation.Init(target);

			await GDTask.Delay(0.5f);
		}
	}

	public virtual GDTask OnRemoved(Condition condition)
	{
		Node?.Destroy();
		Owner.Conditions.Remove(this);

		ScenarioEvents.InflictConditionDuplicatesCheckEvent.Unsubscribe(this);
		ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Unsubscribe(this);

		return GDTask.CompletedTask;
	}

	protected virtual bool DuplicatesCheckCanApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		return !parameters.Prevented && parameters.Target == Owner && parameters.ConditionModel.ImmutableInstance == ImmutableInstance;
	}

	protected virtual GDTask DuplicatesCheckApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		parameters.SetPrevented(true);

		if(parameters.Target.TakingTurn)
		{
			_appliedDuringThisTurn = true;
		}

		return GDTask.CompletedTask;
	}

	protected bool TurnEndedCanApply(ScenarioEvents.FigureTurnEndedConditionsFallOff.Parameters parameters)
	{
		return parameters.Figure == Owner;
	}

	protected async GDTask TurnEndedApply(ScenarioEvents.FigureTurnEndedConditionsFallOff.Parameters parameters)
	{
		if(_appliedDuringThisTurn)
		{
			_appliedDuringThisTurn = false;
		}
		else
		{
			await AbilityCmd.RemoveCondition(Owner, ImmutableInstance);
		}
	}
}
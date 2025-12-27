using Fractural.Tasks;

public abstract class Empower : ConditionModel
{
	public override string Name => "Empower";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Empower.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Positive;
	public override bool CanBeAppliedMultipleTimesInSingleTarget => true;
	public override bool ImmediatelyRemovedOnApply => true;
	public override bool ShouldShowOnFigure => false;
	protected override string ConditionAnimationScenePath => "res://Scenes/Scenario/ConditionAnimations/EmpowerAnimation.tscn";

	// private IHasEmpower EmpowerOwner { get; set; }

	// public void SetEmpowerOwner(IHasEmpower empowerOwner)
	// {
	// 	EmpowerOwner = empowerOwner;
	// }

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		if(EmpowerOwner != null)
		{
			await GameController.Instance.AMDManager.Empower(EmpowerOwner, condition.Owner);
		}
	}
}
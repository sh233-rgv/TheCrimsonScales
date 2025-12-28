using Fractural.Tasks;

public abstract class Empower : ConditionModel
{
	public override string Name => "Empower";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Empower.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Positive;
	public override bool CanBeAppliedMultipleTimesOnSingleTarget => true;
	public override bool ImmediatelyRemovedOnApply => true;
	public override bool RequiresGiver => true;
	public override bool ShouldShowOnFigure => false;
	protected override string ConditionAnimationScenePath => "res://Scenes/Scenario/ConditionAnimations/EmpowerAnimation.tscn";

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		//TODO: Currently expects the giver to have empowers, should probably retrace to original owner of the ability card used to perform the ability? 
		if(condition.PotentialGiver is IHasEmpower hasEmpower)
		{
			await GameController.Instance.AMDManager.Empower(hasEmpower, condition.Owner);
		}
	}
}
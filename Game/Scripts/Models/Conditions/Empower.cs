using Fractural.Tasks;

public abstract class Empower : ConditionModel
{
	public override string Name => "Empower";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Empower.svg";
	public override bool CanStack => true;
	public override bool IsPositive => true;
	protected override string ConditionAnimationScenePath => "res://Scenes/Scenario/ConditionAnimations/EmpowerAnimation.tscn";

	private IHasEmpower EmpowerOwner { get; set; }

	public void SetEmpowerOwner(IHasEmpower empowerOwner)
	{
		EmpowerOwner = empowerOwner;
	}

	public override bool ShouldShowOnFigure(Figure figure)
	{
		return false;
	}

	public override async GDTask Add(Figure target, ConditionNode node)
	{
		await base.Add(target, node);

		if(EmpowerOwner != null)
		{
			await GameController.Instance.AMDManager.Empower(EmpowerOwner, target);
		}

		await AbilityCmd.RemoveCondition(target, this);
	}
}
using Fractural.Tasks;

public class Specialist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Specialist";
	public override string Description => "Never perform a basic action.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
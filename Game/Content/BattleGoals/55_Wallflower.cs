using Fractural.Tasks;

public class Wallflower : TheCrimsonScalesBattleGoal
{
	public override string Title => "Wallflower";
	public override string Description => "End each of your turns adjacent to a wall, obstacle or objective.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
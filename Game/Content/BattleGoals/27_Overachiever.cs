using Fractural.Tasks;

public class Overachiever : TheCrimsonScalesBattleGoal
{
	public override string Title => "Overachiever";
	public override string Description => "Kill an enemy and open a door in the same turn, in either order.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
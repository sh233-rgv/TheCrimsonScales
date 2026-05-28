using Fractural.Tasks;

public class Peacemonger : TheCrimsonScalesBattleGoal
{
	public override string Title => "Peacemonger";
	public override string Description => "Never kill an enemy.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
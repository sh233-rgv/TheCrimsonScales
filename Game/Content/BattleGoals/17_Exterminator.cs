using Fractural.Tasks;

public class Exterminator : TheCrimsonScalesBattleGoal
{
	public override string Title => "Exterminator";
	public override string Description => "Kill one or more enemies of each type that appears in the scenario.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
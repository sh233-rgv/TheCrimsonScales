using Fractural.Tasks;

public class Duelist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Duelist";
	public override string Description => "Never exit a hex adjacent to the enemy except through forced movement.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
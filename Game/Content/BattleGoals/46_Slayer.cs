using Fractural.Tasks;

public class Slayer : TheCrimsonScalesBattleGoal
{
	public override string Title => "Slayer";
	public override string Description => "Kill two or more enemies in the same round.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
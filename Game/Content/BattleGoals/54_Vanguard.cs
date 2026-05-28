using Fractural.Tasks;

public class Vanguard : TheCrimsonScalesBattleGoal
{
	public override string Title => "Vanguard";
	public override string Description => "Never attack an enemy that has already acted in the round.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
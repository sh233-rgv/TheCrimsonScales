using Fractural.Tasks;

public class Slowpoke : TheCrimsonScalesBattleGoal
{
	public override string Title => "Slowpoke";
	public override string Description => "Move no more than two hexes on each turn.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
using Fractural.Tasks;

public class Miser : TheCrimsonScalesBattleGoal
{
	public override string Title => "Miser";
	public override string Description => "Never exit a room with loot tokens in it.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
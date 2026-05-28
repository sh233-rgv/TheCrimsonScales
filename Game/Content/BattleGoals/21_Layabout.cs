using Fractural.Tasks;

public class Layabout : TheCrimsonScalesBattleGoal
{
	public override string Title => "Layabout";
	public override string Description => "Gain 7 or fewer experience before any bonus scenario experience.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
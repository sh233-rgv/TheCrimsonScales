using Fractural.Tasks;

public class Daredevil : TheCrimsonScalesBattleGoal
{
	public override string Title => "Daredevil";
	public override string Description => "Add two cards to your lost pile before your first rest.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
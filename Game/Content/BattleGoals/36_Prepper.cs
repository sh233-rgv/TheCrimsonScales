using Fractural.Tasks;

public class Prepper : TheCrimsonScalesBattleGoal
{
	public override string Title => "Prepper";
	public override string Description => "Perform no attack abilities in the first three rounds.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
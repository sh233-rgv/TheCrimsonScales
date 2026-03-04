using Fractural.Tasks;

public class Discriminating : TheCrimsonScalesBattleGoal
{
	public override string Title => "Discriminating"; // Plebeian in GH2
	public override string Description => "Never kill an elite enemy, named enemy, or boss.";

	public override bool FailIfProgressFull => true;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
using Fractural.Tasks;

public class Opener : TheCrimsonScalesBattleGoal
{
	public override string Title => "Opener";
	public override string Description => "Kill the first enemy to die in the scenario.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
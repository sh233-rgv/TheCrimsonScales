using Fractural.Tasks;

public class Closer : TheCrimsonScalesBattleGoal
{
	public override string Title => "Closer";
	public override string Description => "Kill the last enemy to die in the scenario.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
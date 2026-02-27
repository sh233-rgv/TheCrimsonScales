using Fractural.Tasks;

public class Discriminating : TheCrimsonScalesBattleGoal
{
	public override string Title => "Discriminating";
	public override string Description => "Kill no elite, named or boss monsters during the scenario.";

	public override bool FailIfProgressFull => true;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
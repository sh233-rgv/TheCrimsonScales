using Fractural.Tasks;

public class Discriminating : TheCrimsonScalesBattleGoal
{
	public override string Title => "Discriminating";
	public override string Description => "Kill no elite, named or boss monsters during the scenario.";

	public override bool FailIfProgressFull => true;

	protected override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
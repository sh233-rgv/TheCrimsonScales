using Fractural.Tasks;

public class Diehard : TheCrimsonScalesBattleGoal
{
	public override string Title => "Diehard";

	public override string Description =>
		"Never allow your current hit point value to drop below half your maximum hit point value (rounded up) during the scenario.";

	public override bool FailIfProgressFull => true;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
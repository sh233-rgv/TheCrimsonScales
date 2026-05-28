using Fractural.Tasks;

public class Optimist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Optimist";
	public override string Description => "Remove a negative condition from yourself or an ally two or more times.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
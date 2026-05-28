using Fractural.Tasks;

public class Sadist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Sadist";
	public override string Description => "Kill five or more enemies.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
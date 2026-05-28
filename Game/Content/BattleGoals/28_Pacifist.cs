using Fractural.Tasks;

public class Pacifist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Pacifist";
	public override string Description => "Kill three or fewer enemies.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
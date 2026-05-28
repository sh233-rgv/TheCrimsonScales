using Fractural.Tasks;

public class Straggler : TheCrimsonScalesBattleGoal
{
	public override string Title => "Straggler";
	public override string Description => "Never short rest.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
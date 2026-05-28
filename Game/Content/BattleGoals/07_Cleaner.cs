using Fractural.Tasks;

public class Cleaner : TheCrimsonScalesBattleGoal
{
	public override string Title => "Cleaner";
	public override string Description => "Collect three or more loot tokens in the same turn.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
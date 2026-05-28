using Fractural.Tasks;

public class Conservator : TheCrimsonScalesBattleGoal
{
	public override string Title => "Conservator";
	public override string Description => "Never perform an action with a lost icon.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
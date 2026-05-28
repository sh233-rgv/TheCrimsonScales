using Fractural.Tasks;

public class Tormentor : TheCrimsonScalesBattleGoal
{
	public override string Title => "Tormentor";
	public override string Description => "Apply a different negative condition to an enemy that already has one or more negative conditions.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
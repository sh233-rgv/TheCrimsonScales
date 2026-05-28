using Fractural.Tasks;

public class Ascetic : TheCrimsonScalesBattleGoal
{
	public override string Title => "Ascetic";
	public override string Description => "Collect fewer loot tokens than any other character.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
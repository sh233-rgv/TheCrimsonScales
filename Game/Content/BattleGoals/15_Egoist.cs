using Fractural.Tasks;

public class Egoist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Egoist";
	public override string Description => "Collect more loot tokens than any other character.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
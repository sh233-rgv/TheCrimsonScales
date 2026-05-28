using Fractural.Tasks;

public class Pickpocket : TheCrimsonScalesBattleGoal
{
	public override string Title => "Pickpocket";
	public override string Description => "Collect two or more loot tokens by performing a loot ability while adjacent to one or more enemies.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
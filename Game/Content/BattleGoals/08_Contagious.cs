using Fractural.Tasks;

public class Contagious : TheCrimsonScalesBattleGoal
{
	public override string Title => "Contagious";
	public override string Description => "While afflicted by a negative condition, apply any negative condition to a monster.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
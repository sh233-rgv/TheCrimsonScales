using Fractural.Tasks;

public class Assassin : TheCrimsonScalesBattleGoal
{
	public override string Title => "Assassin";
	public override string Description => "Kill a monster before it takes any actions in the scenario.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
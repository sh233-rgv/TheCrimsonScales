using Fractural.Tasks;

public class Bully : TheCrimsonScalesBattleGoal
{
	public override string Title => "Bully";
	public override string Description => "Kill a monster afflicted by a negative condition";

	protected override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
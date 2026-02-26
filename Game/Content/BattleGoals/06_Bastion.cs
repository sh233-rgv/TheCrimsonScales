using Fractural.Tasks;

public class Bastion : TheCrimsonScalesBattleGoal
{
	public override string Title => "Bastion";
	public override string Description => "Be adjacent to at least two monsters while standing on a door hex.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
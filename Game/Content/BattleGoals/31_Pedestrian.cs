using Fractural.Tasks;

public class Pedestrian : TheCrimsonScalesBattleGoal
{
	public override string Title => "Pedestrian";
	public override string Description => "Never enter a hex occupied by an ally, enemy, objective or obstacle.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
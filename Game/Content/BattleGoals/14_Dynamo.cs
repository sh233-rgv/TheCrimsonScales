using Fractural.Tasks;

public class Dynamo : TheCrimsonScalesBattleGoal
{
	public override string Title => "Dynamo";
	public override string Description => "Kill an enemy with an attack that would have caused at least 4 more points of damage than necessary.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
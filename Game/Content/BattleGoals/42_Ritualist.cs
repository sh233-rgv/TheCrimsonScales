using Fractural.Tasks;

public class Ritualist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Ritualist";
	public override string Description => "Kill an enemy while three or more elements are strong or waning.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
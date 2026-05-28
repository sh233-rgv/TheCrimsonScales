using Fractural.Tasks;

public class Streamliner : TheCrimsonScalesBattleGoal
{
	public override string Title => "Streamliner";
	public override string Description => "Have five or more total cards in your hand and discard pile at the end of the scenario.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
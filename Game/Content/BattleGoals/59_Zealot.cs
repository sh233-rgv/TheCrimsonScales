using Fractural.Tasks;

public class Zealot : TheCrimsonScalesBattleGoal
{
	public override string Title => "Zealot";

	public override string Description =>
		"Have three or fewer total cards in your hand and discard pile while also not exhausted at the end of the scenario.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
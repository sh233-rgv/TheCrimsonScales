using Fractural.Tasks;

public class Executioner : TheCrimsonScalesBattleGoal
{
	public override string Title => "Executioner";
	public override string Description => "Kill an undamaged enemy with a single attack action.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
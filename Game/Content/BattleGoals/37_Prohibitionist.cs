using Fractural.Tasks;

public class Prohibitionist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Prohibitionist";
	public override string Description => "Never use a potion.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
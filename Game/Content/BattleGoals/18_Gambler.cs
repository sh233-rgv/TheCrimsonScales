using Fractural.Tasks;

public class Gambler : TheCrimsonScalesBattleGoal
{
	public override string Title => "Gambler";
	public override string Description => "Kill an enemy with an attack that has disadvantage.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
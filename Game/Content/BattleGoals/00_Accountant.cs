using Fractural.Tasks;

public class Accountant : TheCrimsonScalesBattleGoal
{
	public override string Title => "Accountant";
	public override string Description => "Have zero cards in your hand each time you rest.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
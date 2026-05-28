using Fractural.Tasks;

public class Sleeper : TheCrimsonScalesBattleGoal
{
	public override string Title => "Sleeper";
	public override string Description => "Have one or more cards in your hand each time you rest.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
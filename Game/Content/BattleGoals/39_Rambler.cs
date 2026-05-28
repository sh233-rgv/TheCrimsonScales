using Fractural.Tasks;

public class Rambler : TheCrimsonScalesBattleGoal
{
	public override string Title => "Rambler";
	public override string Description => "End no more than three of your turns in the hex in which you started the turn, except when long resting.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
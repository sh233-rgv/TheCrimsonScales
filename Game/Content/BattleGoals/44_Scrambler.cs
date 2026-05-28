using Fractural.Tasks;

public class Scrambler : TheCrimsonScalesBattleGoal
{
	public override string Title => "Scrambler";
	public override string Description => "Never long rest.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
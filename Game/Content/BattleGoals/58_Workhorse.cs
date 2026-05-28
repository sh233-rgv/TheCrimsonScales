using Fractural.Tasks;

public class Workhorse : TheCrimsonScalesBattleGoal
{
	public override string Title => "Workhorse";
	public override string Description => "Gain 13 or more experience before any bonus scenario experience.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
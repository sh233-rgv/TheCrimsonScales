using Fractural.Tasks;

public class Weakling : TheCrimsonScalesBattleGoal
{
	public override string Title => "Weakling";
	public override string Description => "Become exhausted before any other character.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
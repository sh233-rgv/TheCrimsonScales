using Fractural.Tasks;

public class Hunter : TheCrimsonScalesBattleGoal
{
	public override string Title => "Hunter";
	public override string Description => CharacterCount == 4 ? "Kill two or more elite enemies." : "Kill one or more elite enemies.";

	public override int MaxProgress => CharacterCount == 4 ? 2 : 1;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
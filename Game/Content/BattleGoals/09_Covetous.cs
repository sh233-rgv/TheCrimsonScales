using Fractural.Tasks;

public class Covetous : TheCrimsonScalesBattleGoal
{
	public override string Title => "Covetous";
	public override string Description => "Never collect a money token from end-of-turn looting.";

	public override bool FailIfProgressFull => true;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
using Fractural.Tasks;

public class Pincushion : TheCrimsonScalesBattleGoal
{
	public override string Title => "Pincushion";
	public override string Description => "Be targeted by attacks from three or more enemies in the same round.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
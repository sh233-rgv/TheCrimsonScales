using Fractural.Tasks;

public class Insomniac : TheCrimsonScalesBattleGoal
{
	public override string Title => "Insomniac";
	public override string Description => "Suffer damage from an attack in the same round you long rest.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
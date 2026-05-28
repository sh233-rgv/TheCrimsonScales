using Fractural.Tasks;

public class Ravager : TheCrimsonScalesBattleGoal
{
	public override string Title => "Ravager";
	public override string Description => "Perform two actions with lost icons in the same turn.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
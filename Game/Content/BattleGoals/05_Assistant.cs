using Fractural.Tasks;

public class Assistant : TheCrimsonScalesBattleGoal
{
	public override string Title => "Assistant";
	public override string Description => "Kill a monster attacked by an ally earlier in the round.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
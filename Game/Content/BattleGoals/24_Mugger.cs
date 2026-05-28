using Fractural.Tasks;

public class Mugger : TheCrimsonScalesBattleGoal
{
	public override string Title => "Mugger";
	public override string Description => "Kill an enemy and loot the loot token it drops in the same round.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
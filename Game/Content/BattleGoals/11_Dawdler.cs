using Fractural.Tasks;

public class Dawdler : TheCrimsonScalesBattleGoal
{
	public override string Title => "Dawdler";
	public override string Description => "Never use your lowest initiative played card as your initiative card.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
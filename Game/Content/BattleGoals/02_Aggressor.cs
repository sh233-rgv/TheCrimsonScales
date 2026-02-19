using Fractural.Tasks;

public class Aggressor : TheCrimsonScalesBattleGoal
{
	public override string Title => "Aggressor";
	public override string Description => "Have one or more monsters present on the map at the beginning of every round during the scenario.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override bool FailIfProgressFull => true;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
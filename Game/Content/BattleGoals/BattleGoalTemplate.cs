using Fractural.Tasks;

public class BattleGoalTemplate : TheCrimsonScalesBattleGoal
{
	public override string Title => "TODO";
	public override string Description => "TODO";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two; //TODO: Remove if only One

	public override int MaxProgress => 1; //TODO: Change if different max progress
	public override bool FailIfProgressFull => true; //TODO: Remove if it's a normal Battle Goal that just completes if the progress is full

	protected override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
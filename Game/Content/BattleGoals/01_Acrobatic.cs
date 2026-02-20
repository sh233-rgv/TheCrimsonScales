using Fractural.Tasks;

public class Acrobatic : TheCrimsonScalesBattleGoal
{
	public override string Title => "Acrobatic";
	public override string Description => "Lose a card to negate 5 or more damage.";

	protected override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
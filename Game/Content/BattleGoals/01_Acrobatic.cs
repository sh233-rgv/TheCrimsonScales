using Fractural.Tasks;

public class Acrobatic : TheCrimsonScalesBattleGoal
{
	public override string Title => "Acrobatic";
	public override string Description => "Lose a card to negate 5 or more damage.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
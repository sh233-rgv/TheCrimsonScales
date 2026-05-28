using Fractural.Tasks;

public class Wastrel : TheCrimsonScalesBattleGoal
{
	public override string Title => "Wastrel";
	public override string Description => "Lose a card to negate 2 or less damage from an attack.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
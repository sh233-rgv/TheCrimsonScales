using Fractural.Tasks;

public class Recluse : TheCrimsonScalesBattleGoal
{
	public override string Title => "Accountant";
	public override string Description => "Never end your turn adjacent to any other character.";

	public override BattleGoalCheckmarkCount CheckmarkCount => CharacterCount == 4 ? BattleGoalCheckmarkCount.Two : BattleGoalCheckmarkCount.One;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
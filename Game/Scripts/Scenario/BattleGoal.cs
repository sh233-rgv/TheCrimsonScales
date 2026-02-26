using Fractural.Tasks;

public class BattleGoal
{
	public BattleGoalModel Model { get; }
	public BattleGoalData Data { get; }

	public BattleGoal(BattleGoalModel model)
	{
		Model = model;
		Data = new BattleGoalData();
	}

	public async GDTask OnScenarioSetupPhaseCompleted(Character character)
	{
		await Model.OnScenarioSetupPhaseCompleted(character, Data);
	}
}
public class ScenarioUnlockEventReward(ScenarioModel scenarioModel) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Unlock Scenario {scenarioModel.ScenarioNumber}.";
}
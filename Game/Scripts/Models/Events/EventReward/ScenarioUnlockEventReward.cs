using Fractural.Tasks;

public class ScenarioUnlockEventReward(ScenarioModel scenarioModel) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Unlock Scenario {scenarioModel.ScenarioNumber}.";

	public override async GDTask Resolve()
	{
		await base.Resolve();

		//TODO: Unlock scenario
	}
}
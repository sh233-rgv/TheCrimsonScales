using Fractural.Tasks;
using Godot;

public class UnlockScenarioReward(ScenarioModel scenarioModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Unlock Scenario {scenarioModel.ScenarioNumber}.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

		savedCampaign.SavedScenarioProgresses.GetScenarioProgress(scenarioModel).Discover();
	}
}
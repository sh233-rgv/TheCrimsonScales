using System.Threading;
using Fractural.Tasks;

public class UnlockScenarioReward(ScenarioModel scenarioModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Unlock Scenario {scenarioModel.ScenarioNumber}.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		savedCampaign.SavedScenarioProgresses.GetScenarioProgress(scenarioModel).Discover();
	}
}
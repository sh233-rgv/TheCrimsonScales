using Fractural.Tasks;
using Godot;

public class UnlockScenarioReward(ScenarioModel scenarioModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Unlock Scenario {scenarioModel.ScenarioNumber}.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		BetweenScenariosController.Instance.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(scenarioModel).Discover();
	}
}
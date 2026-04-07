using Fractural.Tasks;
using Godot;

public class LoseReputationReward(int reputationAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Lose {reputationAmount} reputation.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		BetweenScenariosController.Instance.SavedCampaign.AdjustReputation(-reputationAmount);
	}
}
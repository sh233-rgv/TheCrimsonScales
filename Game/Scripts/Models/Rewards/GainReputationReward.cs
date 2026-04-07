using Fractural.Tasks;
using Godot;

public class GainReputationReward(int reputationAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain {reputationAmount} reputation.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

		savedCampaign.AdjustReputation(reputationAmount);
	}
}
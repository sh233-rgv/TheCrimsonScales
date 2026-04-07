using System.Threading;
using Fractural.Tasks;
using Godot;

public class GainReputationReward(int reputationAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain {reputationAmount} reputation.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		savedCampaign.AdjustReputation(reputationAmount);
	}
}
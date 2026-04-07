using System.Threading;
using Fractural.Tasks;
using Godot;

public class LoseProsperityReward(int prosperity) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Lose {prosperity} prosperity.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		savedCampaign.AdjustProsperity(-prosperity);
	}
}
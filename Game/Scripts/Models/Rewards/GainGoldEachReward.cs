using System.Threading;
using Fractural.Tasks;
using Godot;

public class GainGoldEachReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain {Icons.Inline(Icons.Coins, parameters)}{goldAmount} each.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		foreach(SavedCharacter savedCharacter in savedCampaign.Characters)
		{
			savedCharacter.AddGold(goldAmount);
		}
	}
}
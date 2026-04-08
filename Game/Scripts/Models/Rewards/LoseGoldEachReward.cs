using System.Threading;
using Fractural.Tasks;
using Godot;

public class LoseGoldEachReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => $"Lose {Icons.Inline(Icons.Coins, textParameters)}{goldAmount} each.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		foreach(SavedCharacter savedCharacter in savedCampaign.Characters)
		{
			savedCharacter.RemoveGold(Mathf.Min(savedCharacter.Gold, goldAmount));
		}
	}
}
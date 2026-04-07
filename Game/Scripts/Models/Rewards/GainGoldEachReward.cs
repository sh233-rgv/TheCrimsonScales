using Fractural.Tasks;
using Godot;

public class GainGoldEachReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain {Icons.Inline(Icons.Coins, parameters)}{goldAmount} each.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

		foreach(SavedCharacter savedCharacter in savedCampaign.Characters)
		{
			savedCharacter.AddGold(goldAmount);
		}
	}
}
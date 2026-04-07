using Fractural.Tasks;
using Godot;

public class GainGoldEachReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain {Icons.Inline(Icons.Coins, parameters)}{goldAmount} each.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		foreach(SavedCharacter savedCharacter in BetweenScenariosController.Instance.SavedCampaign.Characters)
		{
			savedCharacter.AddGold(goldAmount);
		}
	}
}
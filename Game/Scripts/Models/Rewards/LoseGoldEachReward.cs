using Fractural.Tasks;
using Godot;

public class LoseGoldEachReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Lose {Icons.Inline(Icons.Coins, parameters)}{goldAmount} each.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		foreach(SavedCharacter savedCharacter in BetweenScenariosController.Instance.SavedCampaign.Characters)
		{
			savedCharacter.RemoveGold(Mathf.Min(savedCharacter.Gold, goldAmount));
		}
	}
}
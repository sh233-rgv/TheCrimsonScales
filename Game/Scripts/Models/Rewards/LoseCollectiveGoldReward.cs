using System.Linq;
using Fractural.Tasks;
using Godot;

public class LoseCollectiveGoldReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;

	public override string GetLabelText(RichTextParameters parameters) =>
		$"Lose {goldAmount} collective {Icons.Inline(Icons.Coins, parameters)}.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

		int adjustedGoldAmount = Mathf.Min(goldAmount, savedCampaign.Characters.Sum(character => character.Gold));
		AppController.Instance.PopupManager.RequestPopup(new GoldDistributionPopup.Request()
		{
			Gold = adjustedGoldAmount,
			LoseGold = true,
			Characters = savedCampaign.Characters,
		});

		await GDTask.WaitWhile(() => AppController.Instance.PopupManager.IsPopupOpen<GoldDistributionPopup.Request>());
	}
}
using System.Linq;
using System.Threading;
using Fractural.Tasks;
using Godot;

public class LoseCollectiveGoldReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;

	public override string GetLabelText(RichTextParameters textParameters) =>
		$"Lose {goldAmount} collective {Icons.Inline(Icons.Coins, textParameters)}.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		int adjustedGoldAmount = Mathf.Min(goldAmount, savedCampaign.Characters.Sum(character => character.Gold));
		AppController.Instance.PopupManager.RequestPopup(new GoldDistributionPopup.Request()
		{
			Gold = adjustedGoldAmount,
			LoseGold = true,
			Characters = savedCampaign.Characters,
		});

		await GDTask.WaitWhile(() => AppController.Instance.PopupManager.IsPopupOpen<GoldDistributionPopup.Request>(),
			cancellationToken: cancellationToken);
	}
}
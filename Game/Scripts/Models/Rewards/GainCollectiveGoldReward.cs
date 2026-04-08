using System.Threading;
using Fractural.Tasks;
using Godot;

public class GainCollectiveGoldReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;

	public override string GetLabelText(RichTextParameters textParameters) =>
		$"Gain {goldAmount} collective {Icons.Inline(Icons.Coins, textParameters)}.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		AppController.Instance.PopupManager.RequestPopup(new GoldDistributionPopup.Request()
		{
			Gold = goldAmount,
			LoseGold = false,
			Characters = savedCampaign.Characters,
		});

		await GDTask.WaitWhile(() => AppController.Instance.PopupManager.IsPopupOpen<GoldDistributionPopup.Request>(),
			cancellationToken: cancellationToken);
	}
}
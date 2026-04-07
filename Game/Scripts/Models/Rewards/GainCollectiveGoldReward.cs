using Fractural.Tasks;
using Godot;

public class GainCollectiveGoldReward(int goldAmount) : Reward
{
	public override RewardType Type => RewardType.Immediate;

	public override string GetLabelText(RichTextParameters parameters) =>
		$"Gain {goldAmount} collective {Icons.Inline(Icons.Coins, parameters)}.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		AppController.Instance.PopupManager.RequestPopup(new GoldDistributionPopup.Request()
		{
			Gold = goldAmount,
			LoseGold = false,
			Characters = BetweenScenariosController.Instance.SavedCampaign.Characters,
		});

		await GDTask.WaitWhile(() => AppController.Instance.PopupManager.IsPopupOpen<GoldDistributionPopup.Request>());
	}
}
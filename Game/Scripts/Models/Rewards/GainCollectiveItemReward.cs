using System.Threading;
using Fractural.Tasks;
using Godot;

public class GainCollectiveItemReward(ItemModel itemModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => $"Gain 1 collective '{itemModel.Name}'.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		AppController.Instance.PopupManager.RequestPopup(new ItemRewardCharacterSelectionPopup.Request()
		{
			ItemModel = itemModel,
			Characters = savedCampaign.Characters,
			OnCharacterConfirmed = character =>
			{
				savedCampaign.GetSavedItem(itemModel).AddUnlocked(1);
				character.AddItem(itemModel);
			}
		});

		await GDTask.WaitWhile(() => AppController.Instance.PopupManager.IsPopupOpen<ItemRewardCharacterSelectionPopup.Request>());
	}
}
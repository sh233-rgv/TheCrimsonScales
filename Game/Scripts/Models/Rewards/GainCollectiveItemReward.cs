using Fractural.Tasks;
using Godot;

public class GainCollectiveItemReward(ItemModel itemModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain 1 collective '{itemModel.Name}'.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

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
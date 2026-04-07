using Fractural.Tasks;
using Godot;

public class GainItemDesignReward(ItemModel itemModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain '{itemModel.Name}' item design.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		SavedItem savedItem = BetweenScenariosController.Instance.SavedCampaign.GetSavedItem(itemModel);
		savedItem.AddUnlocked(1);
		savedItem.AddStock(1);
	}
}
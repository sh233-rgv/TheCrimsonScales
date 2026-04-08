using System.Threading;
using Fractural.Tasks;
using Godot;

public class GainItemDesignReward(ItemModel itemModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => $"Gain '{itemModel.Name}' item design.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		SavedItem savedItem = savedCampaign.GetSavedItem(itemModel);
		savedItem.AddUnlocked(1);
		savedItem.AddStock(1);
	}
}
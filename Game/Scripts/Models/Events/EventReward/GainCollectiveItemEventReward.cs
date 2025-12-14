using Fractural.Tasks;
using Godot;

public class GainCollectiveItemEventReward(ItemModel itemModel) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string GetLabelText(Color textColor) => $"Gain 1 collective {itemModel.Name}.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		//TODO: Open popup to gift item
	}
}
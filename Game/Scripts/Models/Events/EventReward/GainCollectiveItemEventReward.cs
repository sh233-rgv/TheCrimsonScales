public class GainCollectiveItemEventReward(ItemModel itemModel) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Gain 1 collective {itemModel.Name}.";
}
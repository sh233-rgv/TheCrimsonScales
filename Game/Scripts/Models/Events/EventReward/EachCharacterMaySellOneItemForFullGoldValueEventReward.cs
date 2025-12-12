using Fractural.Tasks;

public class EachCharacterMaySellOneItemForFullGoldValueEventReward() : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => "Each character may sell one item to the shop for its full gold value.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		//TODO: sell items
	}
}
using Fractural.Tasks;

public class GainCollectiveGoldEventReward(int goldAmount) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Gain {goldAmount} collective {Icons.Inline(Icons.Coins)}.";

	public override async GDTask Resolve()
	{
		await base.Resolve();

		//TODO: Open popup to distribute gold
	}
}
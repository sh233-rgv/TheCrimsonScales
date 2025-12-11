public class GainCollectiveGoldEventReward(int goldAmount) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Gain {goldAmount} collective {Icons.Inline(Icons.Coins)}.";
}
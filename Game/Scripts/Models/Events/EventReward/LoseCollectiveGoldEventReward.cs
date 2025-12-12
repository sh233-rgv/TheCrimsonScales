using Fractural.Tasks;

public class LoseCollectiveGoldEventReward(int goldAmount) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Lose {goldAmount} collective {Icons.Inline(Icons.Coins)}.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		//TODO: Open popup to distribute gold spending
	}
}
using Fractural.Tasks;
using Godot;

public class LoseCollectiveGoldEventReward(int goldAmount) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string GetLabelText(Color textColor) => $"Lose {goldAmount} collective {Icons.Inline(Icons.Coins, color: textColor)}.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		//TODO: Open popup to distribute gold spending
	}
}
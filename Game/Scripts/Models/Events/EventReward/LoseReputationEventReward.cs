using Fractural.Tasks;

public class LoseReputationEventReward(int reputationAmount) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Lose {reputationAmount} reputation.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		//TODO: Adjust reputation
	}
}
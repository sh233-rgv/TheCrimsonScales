using Fractural.Tasks;

public class GainReputationEventReward(int reputationAmount) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string LabelText => $"Gain {reputationAmount} reputation.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		//TODO: Adjust reputation
	}
}
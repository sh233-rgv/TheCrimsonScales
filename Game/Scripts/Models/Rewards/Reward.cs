using Fractural.Tasks;

public abstract class Reward
{
	public abstract RewardType Type { get; }
	public abstract string GetLabelText(RichTextParameters parameters);

	public virtual async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await GDTask.CompletedTask;
	}

	public virtual void SubscribeDuringDowntime(SavedEventState savedEventState)
	{
	}

	public virtual void UnsubscribeDuringDowntime()
	{
	}

	public virtual async GDTask OnScenarioSetupPhaseCompleted()
	{
		await GDTask.CompletedTask;
	}
}
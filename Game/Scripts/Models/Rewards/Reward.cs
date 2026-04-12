using System;
using System.Threading;
using Fractural.Tasks;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public abstract class Reward
{
	public abstract RewardType Type { get; }

	public abstract string GetLabelText(RichTextParameters textParameters);

	public virtual async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
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
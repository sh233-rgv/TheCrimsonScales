using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class DoNotDrawRoadReward : Reward
{
	public override RewardType Type => RewardType.DuringDowntime;

	public DoNotDrawRoadReward()
	{
	}

	public override string GetLabelText(RichTextParameters textParameters) => "Do not draw a road event.";

	public override void SubscribeDuringDowntime(SavedEventState savedEventState)
	{
		base.SubscribeDuringDowntime(savedEventState);

		BetweenScenariosEvents.DrawRoadEventEvent.Subscribe(this,
			parameters =>
			{
				parameters.SetDrawEvent(false);
				savedEventState.Complete(this);
			}
		);
	}

	public override void UnsubscribeDuringDowntime()
	{
		base.UnsubscribeDuringDowntime();

		BetweenScenariosEvents.DrawRoadEventEvent.Unsubscribe(this);
	}
}
using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public abstract class DowntimeEnhancementCostReward : Reward
{
	public override RewardType Type => RewardType.DuringDowntime;

	public override void SubscribeDuringDowntime(SavedEventState savedEventState)
	{
		base.SubscribeDuringDowntime(savedEventState);

		BetweenScenariosEvents.CalculateEnhancementCostEvent.Subscribe(this, CalculateCostApplyFunction);
		BetweenScenariosEvents.EnhancementBoughtEvent.Subscribe(this, EnhancementBoughtApplyFunction);
	}

	public override void UnsubscribeDuringDowntime()
	{
		base.UnsubscribeDuringDowntime();

		BetweenScenariosEvents.CalculateEnhancementCostEvent.Unsubscribe(this);
		BetweenScenariosEvents.EnhancementBoughtEvent.Unsubscribe(this);
	}

	protected abstract void CalculateCostApplyFunction(BetweenScenariosEvents.CalculateEnhancementCost.Parameters parameters);

	protected abstract void EnhancementBoughtApplyFunction(BetweenScenariosEvents.EnhancementBought.Parameters parameters);
}
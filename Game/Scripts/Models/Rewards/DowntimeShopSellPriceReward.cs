using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public abstract class DowntimeShopSellPriceReward : Reward
{
	public override RewardType Type => RewardType.DuringDowntime;

	public override void SubscribeDuringDowntime(SavedEventState savedEventState)
	{
		base.SubscribeDuringDowntime(savedEventState);

		BetweenScenariosEvents.CalculateItemSellPriceEvent.Subscribe(this, CalculatePriceApplyFunction);
		BetweenScenariosEvents.ItemSoldEvent.Subscribe(this, ItemSoldApplyFunction);
	}

	public override void UnsubscribeDuringDowntime()
	{
		base.UnsubscribeDuringDowntime();

		BetweenScenariosEvents.CalculateItemSellPriceEvent.Unsubscribe(this);
		BetweenScenariosEvents.ItemSoldEvent.Unsubscribe(this);
	}

	protected abstract void CalculatePriceApplyFunction(BetweenScenariosEvents.CalculateItemSellPrice.Parameters parameters);

	protected abstract void ItemSoldApplyFunction(BetweenScenariosEvents.ItemSold.Parameters parameters);
}
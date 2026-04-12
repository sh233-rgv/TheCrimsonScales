// using System;
//
// public class DowntimeShopSellPriceReward(
// 	Func<Reward, BetweenScenariosEvents.CalculateItemSellPrice.ApplyFunction> calculatePriceApplyFunction,
// 	Func<Reward, BetweenScenariosEvents.ItemSold.ApplyFunction> itemSoldApplyFunction,
// 	Func<RichTextParameters, string> getLabelText)
// 	: Reward
// {
// 	public override RewardType Type => RewardType.DuringDowntime;
// 	public override string GetLabelText(RichTextParameters textParameters) => getLabelText(textParameters);
//
// 	public override void SubscribeDuringDowntime(SavedEventState savedEventState)
// 	{
// 		base.SubscribeDuringDowntime(savedEventState);
//
// 		BetweenScenariosEvents.CalculateItemSellPriceEvent.Subscribe(this, calculatePriceApplyFunction(this));
// 		BetweenScenariosEvents.ItemSoldEvent.Subscribe(this, itemSoldApplyFunction(this));
// 	}
//
// 	public override void UnsubscribeDuringDowntime()
// 	{
// 		base.UnsubscribeDuringDowntime();
//
// 		BetweenScenariosEvents.CalculateItemSellPriceEvent.Unsubscribe(this);
// 		BetweenScenariosEvents.ItemSoldEvent.Unsubscribe(this);
// 	}
// }


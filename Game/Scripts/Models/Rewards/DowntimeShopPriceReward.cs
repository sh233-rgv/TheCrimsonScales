using System;
using Godot;

public class DowntimeShopPriceReward(
	Func<Reward, BetweenScenariosEvents.CalculateItemBuyPrice.ApplyFunction> calculatePriceApplyFunction,
	Func<Reward, BetweenScenariosEvents.ItemBought.ApplyFunction> itemBoughtApplyFunction,
	Func<RichTextParameters, string> getLabelText)
	: Reward
{
	public override RewardType Type => RewardType.DuringDowntime;
	public override string GetLabelText(RichTextParameters parameters) => getLabelText(parameters);

	public override void SubscribeDuringDowntime(SavedEventState savedEventState)
	{
		base.SubscribeDuringDowntime(savedEventState);

		BetweenScenariosEvents.CalculateItemBuyPriceEvent.Subscribe(this, calculatePriceApplyFunction(this));
		BetweenScenariosEvents.ItemBoughtEvent.Subscribe(this, itemBoughtApplyFunction(this));
	}

	public override void UnsubscribeDuringDowntime()
	{
		base.UnsubscribeDuringDowntime();

		BetweenScenariosEvents.CalculateItemBuyPriceEvent.Unsubscribe(this);
		BetweenScenariosEvents.ItemBoughtEvent.Unsubscribe(this);
	}
}
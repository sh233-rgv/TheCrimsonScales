using System;
using Godot;

public class DowntimeEnhancementCostReward(
	Func<Reward, BetweenScenariosEvents.CalculateEnhancementCost.ApplyFunction> calculateCostApplyFunction,
	Func<Reward, BetweenScenariosEvents.EnhancementBought.ApplyFunction> enhancementBoughtApplyFunction,
	Func<RichTextParameters, string> getLabelText)
	: Reward
{
	public override RewardType Type => RewardType.DuringDowntime;
	public override string GetLabelText(RichTextParameters parameters) => getLabelText(parameters);

	public override void SubscribeDuringDowntime(SavedEventState savedEventState)
	{
		base.SubscribeDuringDowntime(savedEventState);

		BetweenScenariosEvents.CalculateEnhancementCostEvent.Subscribe(this, calculateCostApplyFunction(this));
		BetweenScenariosEvents.EnhancementBoughtEvent.Subscribe(this, enhancementBoughtApplyFunction(this));
	}

	public override void UnsubscribeDuringDowntime()
	{
		base.UnsubscribeDuringDowntime();

		BetweenScenariosEvents.CalculateEnhancementCostEvent.Unsubscribe(this);
		BetweenScenariosEvents.EnhancementBoughtEvent.Unsubscribe(this);
	}
}
using System.Collections.Generic;

public class AddCityAndRoad60MerchantsGuildHallReward : MerchantsGuildHallRewardModel
{
	public override string GetDescription(RichTextParameters richTextParameters) =>
		"Add City and Road Event 60 to the top of the corresponding decks";

	public override List<Reward> GetRewards() =>
	[
		new AddCityReward(ModelDB.Event<City60>()),
		new AddRoadReward(ModelDB.Event<Road60>()),
	];
}
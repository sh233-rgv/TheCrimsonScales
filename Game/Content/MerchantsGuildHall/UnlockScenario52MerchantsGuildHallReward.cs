using System.Collections.Generic;

public class UnlockScenario52MerchantsGuildHallReward : MerchantsGuildHallRewardModel
{
	public override string GetDescription(RichTextParameters richTextParameters) => "Unlock Scenario 52: “Rodent Warehouse”";

	public override List<Reward> GetRewards() =>
	[
		new UnlockScenarioReward(ModelDB.Scenario<Scenario052>())
	];
}
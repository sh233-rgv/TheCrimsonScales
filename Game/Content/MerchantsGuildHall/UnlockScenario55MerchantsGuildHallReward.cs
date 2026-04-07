using System.Collections.Generic;

public class UnlockScenario55MerchantsGuildHallReward : MerchantsGuildHallRewardModel
{
	public override string GetDescription(RichTextParameters richTextParameters) => "Unlock Scenario 55: “Catacomb Plunder”";

	public override List<Reward> GetRewards() =>
	[
		new UnlockScenarioReward(ModelDB.Scenario<Scenario055>())
	];
}
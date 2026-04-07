using System.Collections.Generic;

public class UnlockScenario53MerchantsGuildHallReward : MerchantsGuildHallRewardModel
{
	public override string GetDescription(RichTextParameters richTextParameters) => "Unlock Scenario 53: “Cave of Currents”";

	public override List<Reward> GetRewards() =>
	[
		new UnlockScenarioReward(ModelDB.Scenario<Scenario053>())
	];
}
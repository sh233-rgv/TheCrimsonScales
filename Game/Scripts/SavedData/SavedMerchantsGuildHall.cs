using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class SavedMerchantsGuildHall
{
	private static readonly MerchantsGuildHallRewardModel[] AllRewards =
	[
	];

	[JsonProperty]
	public List<SavedMerchantsGuildHallReward> Rewards { get; private set; }

	[JsonProperty]
	public int CompletedScenarioCount { get; private set; }

	public SavedMerchantsGuildHall()
	{
		Rewards = AllRewards.Select(rewardModel => new SavedMerchantsGuildHallReward(rewardModel)).ToList();
	}

	public void AddCompletedScenario()
	{
		CompletedScenarioCount++;
	}
}
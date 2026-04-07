using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedMerchantsGuildHallReward
{
	[JsonProperty]
	public string ModelId { get; private set; }

	[JsonProperty]
	public bool Unlocked { get; private set; }

	public MerchantsGuildHallRewardModel Model => ModelDB.GetById<MerchantsGuildHallRewardModel>(ModelId);

	public SavedMerchantsGuildHallReward()
	{
	}

	public SavedMerchantsGuildHallReward(MerchantsGuildHallRewardModel model)
	{
		ModelId = model.Id.ToString();
	}
}
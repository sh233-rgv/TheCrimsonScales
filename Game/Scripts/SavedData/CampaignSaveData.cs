using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class CampaignSaveData : SaveData
{
	[JsonProperty]
	public Guid CampaignId { get; set; }

	[JsonProperty]
	public SavedCampaign SavedCampaign { get; set; }

	[JsonProperty]
	public SavedCampaignOptions Options { get; set; } = new SavedCampaignOptions();
}
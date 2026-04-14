using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedCampaignOptions
{
	[JsonProperty]
	public SavedOption<int> Difficulty { get; private set; } = new SavedOption<int>(0);
}
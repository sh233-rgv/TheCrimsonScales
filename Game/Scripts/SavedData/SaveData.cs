using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SaveData
{
	[JsonProperty]
	public int MigrationVersion { get; set; }

	[JsonProperty]
	public string AppVersion { get; set; }

	[JsonProperty]
	public Guid PlayerId { get; set; }

	[JsonProperty]
	public SavedOptions Options { get; set; } = new SavedOptions();

	[JsonProperty]
	public SavedCampaign SavedCampaign { get; set; }
}
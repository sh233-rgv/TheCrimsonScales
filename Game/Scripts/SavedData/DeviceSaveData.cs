using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class DeviceSaveData : SaveData
{
	[JsonProperty]
	public Guid DeviceId { get; set; }

	[JsonProperty]
	public SavedDeviceOptions Options { get; set; } = new SavedDeviceOptions();
}
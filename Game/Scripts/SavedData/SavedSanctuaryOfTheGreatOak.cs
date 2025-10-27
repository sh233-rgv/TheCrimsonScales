using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedSanctuaryOfTheGreatOak
{
	[JsonProperty]
	public int TotalDonationCount { get; private set; }

	[JsonProperty]
	public List<string> CritAMDCardIds { get; private set; }

	[JsonProperty]
	public List<string> RollingAMDCardIds { get; private set; }

	public SavedSanctuaryOfTheGreatOak()
	{
		TotalDonationCount = 0;
		CritAMDCardIds = [];
	}

	public void Donate()
	{
		TotalDonationCount++;
	}
}
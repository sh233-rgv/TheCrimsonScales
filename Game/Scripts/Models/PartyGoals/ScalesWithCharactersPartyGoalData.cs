using System.Collections.Generic;
using Newtonsoft.Json;

public class ScalesWithCharactersPartyGoalData : PartyGoalData
{
	[JsonProperty]
	private Dictionary<string, int> _characterProgresses = new Dictionary<string, int>();
}
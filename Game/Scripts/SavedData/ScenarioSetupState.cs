using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class ScenarioSetupState
{
	[JsonProperty]
	public CharacterScenarioSetupState[] CharacterScenarioSetupStates { get; set; }

	[JsonProperty]
	public bool Completed { get; set; }
}
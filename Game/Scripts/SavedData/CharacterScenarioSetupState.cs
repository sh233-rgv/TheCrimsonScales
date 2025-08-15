using System;
using Godot;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class CharacterScenarioSetupState
{
	// The cards the character is bringing to the scenario
	// [JsonProperty]
	// public List<int> HandCardReferenceIds { get; set; }

	[JsonProperty]
	public Vector2I StartHexCoords { get; set; }
}
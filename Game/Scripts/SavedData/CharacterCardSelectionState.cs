using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class CharacterCardSelectionState
{
	[JsonProperty]
	public List<int> ChosenCardReferenceIds { get; set; }

	[JsonProperty]
	public bool LongResting { get; set; }

	// [JsonProperty]
	// public Vector2I? StartHexCoords { get; set; }
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class CardSelectionState
{
	[JsonProperty]
	public CharacterCardSelectionState[] CharacterCardSelectionStates { get; set; }

	[JsonProperty]
	public int CurrentPromptIndex { get; set; }

	[JsonProperty]
	public List<SyncedAction> SyncedActions { get; set; }

	[JsonProperty]
	public bool Completed { get; set; }
}
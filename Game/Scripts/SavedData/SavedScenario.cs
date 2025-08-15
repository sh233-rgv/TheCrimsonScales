using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedScenario
{
	[JsonProperty]
	public Guid Id { get; set; }

	[JsonProperty]
	public string AppVersion { get; set; }

	[JsonProperty]
	public string ScenarioModelId { get; set; }

	[JsonProperty]
	public int Seed { get; set; }

	[JsonProperty]
	public int ScenarioLevel { get; set; }

	[JsonProperty]
	public bool IsOnline { get; set; }

	[JsonProperty]
	public ScenarioSetupState ScenarioSetupState { get; set; }

	[JsonProperty]
	public List<CardSelectionState> CardSelectionStates { get; private set; } = new List<CardSelectionState>();

	[JsonProperty]
	public List<PromptAnswer> PromptAnswers { get; private set; } = new List<PromptAnswer>();
}
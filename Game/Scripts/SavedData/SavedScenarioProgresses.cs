using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedScenarioProgresses
{
	[JsonProperty]
	public Dictionary<string, SavedScenarioProgress> ScenarioProgresses { get; private set; } = new Dictionary<string, SavedScenarioProgress>();

	public SavedScenarioProgress GetScenarioProgress(ScenarioModel scenarioModel)
	{
		if(!ScenarioProgresses.TryGetValue(scenarioModel.Id.ToString(), out SavedScenarioProgress savedScenarioProgress))
		{
			savedScenarioProgress = new SavedScenarioProgress()
			{
			};
			ScenarioProgresses.Add(scenarioModel.Id.ToString(), savedScenarioProgress);
		}

		return savedScenarioProgress;
	}
}
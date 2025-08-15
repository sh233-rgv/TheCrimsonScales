public class BetweenScenariosSceneRequest : SceneRequest
{
	public override string ScenePath => "res://Scenes/BetweenScenarios/BetweenScenarios.tscn";

	public SavedCampaign SavedCampaign { get; }
	public string CompletedScenarioModelId { get; }

	public BetweenScenariosSceneRequest(SavedCampaign savedCampaign, string completedScenarioModelId = null)
	{
		SavedCampaign = savedCampaign;
		CompletedScenarioModelId = completedScenarioModelId;
	}
}
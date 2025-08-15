using Godot;

public class InfectiousScenarioChain : ScenarioChain
{
	public override ScenarioChain BaseScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override Color Color => Color.FromHtml("93c07f");
}
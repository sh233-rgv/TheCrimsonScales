using System.Collections.Generic;

public abstract class SoloScenarioModel : ScenarioModel
{
	//TODO: Make solo scenarios only doable once by each character
	public abstract ClassModel ClassModel { get; }
	public override List<ScenarioLink> Links => [GloomhavenLink.Instance];
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<SoloScenarioChain>();
}
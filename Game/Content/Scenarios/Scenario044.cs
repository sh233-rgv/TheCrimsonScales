using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fractural.Tasks;

public class Scenario044 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario044.tscn";
	public override int ScenarioNumber => 44;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<EventScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() =>
		new CustomScenarioGoals($"Kill {GameController.Instance.SavedCampaign.Characters.Count * 2} Living Spirits to win this scenario.");

	

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		
	}
}
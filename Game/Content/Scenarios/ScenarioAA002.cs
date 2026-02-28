using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ScenarioAA002 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioAA002.tscn";
	public override string ScenarioPrefix => "AA";
	public override int ScenarioNumber => 2;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<AAScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() =>
		new KillSpecificEnemiesTypeGoals(ModelDB.Monster<Echo>(), "Kill the Echo to win this scenario.");

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

	}
}
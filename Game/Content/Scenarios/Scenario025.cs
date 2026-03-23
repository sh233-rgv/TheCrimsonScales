using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class Scenario025 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario025.tscn";
	public override int ScenarioNumber => 25;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<WondrousScenarioChain>();
	//public override IEnumerable<ScenarioConnection> Connections => [new ScenarioConnection<Scenario031>()];

	protected override ScenarioGoals CreateScenarioGoals() =>
		new CustomScenarioGoals("Loot at least four Goal treasure tiles and keep the Brightspark alive to win this scenario.");


	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		await SpawnNPC(GameController.Instance.Map.GetMarker(Marker.Type.b).Hex)
	}
}
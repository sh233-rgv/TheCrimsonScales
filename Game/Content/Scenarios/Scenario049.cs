using Fractural.Tasks;

public class Scenario049 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario049.tscn";
	public override int ScenarioNumber => 49;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<EventScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillAlLEnemiesScenarioGoals();

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		
	}
}
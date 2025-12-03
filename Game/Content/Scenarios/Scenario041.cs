using Fractural.Tasks;

public class Scenario041 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario041.tscn";
	public override int ScenarioNumber => 41;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<EventScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillSpecificEnemiesTypeGoals(ModelDB.Monster<BanditArcher>(), "Kill the Rogue Hollowpact to win this scenario.");

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		GameController.Instance.Map.Treasures[0].SetItemLoot(ModelDB.Item<DizzyingTincture>());
	}
}

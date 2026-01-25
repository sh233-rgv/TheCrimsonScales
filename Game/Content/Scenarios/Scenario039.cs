using System.Linq;
using Fractural.Tasks;

public class Scenario039 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario039.tscn";
	public override int ScenarioNumber => 39;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<PersonalQuestScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() =>
		new CustomScenarioGoals("");


	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();
		
		GameController.Instance.Map.Treasures[0].SetItemDesignLoot(ModelDB.Item<VipertoothDagger>());
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters parameters)
	{
		await base.OnRoomRevealed(parameters);
	}
}
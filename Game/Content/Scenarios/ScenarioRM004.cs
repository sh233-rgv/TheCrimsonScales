using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ScenarioRM004 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioRM004.tscn";
	public override string ScenarioPrefix => "RM";
	public override int ScenarioNumber => 4;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<RMScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() =>
		new KillSpecificEnemiesTypeGoals(ModelDB.Monster<TerrorscaleDrakeRM4Room1>(), "Kill the Terrorscale Drake to win this scenario.");

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters roomRevealedParameters)
	{
		await base.OnRoomRevealed(roomRevealedParameters);

		if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[1])
		{
			//TODO
		}
		else if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[3])
		{
			//TODO
		}
	}
}
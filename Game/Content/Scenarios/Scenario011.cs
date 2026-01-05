using System.Collections.Generic;
using Fractural.Tasks;

public class Scenario011 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario011.tscn";
	public override int ScenarioNumber => 11;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new CustomScenarioGoals("Kill all enemies to win this scenario.");

	public override string BGSPath => null;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO: Scenario Effect
		foreach(Room room in GameController.Instance.Map.Rooms)
		{
			ScenarioCheckEvents.CanEnterMapTileCheckEvent.Subscribe(this, room,
				canApplyParameters => canApplyParameters.Figure is Character or Summon &&
				                      canApplyParameters.MapTile != canApplyParameters.Figure.Hex.MapTile,
				applyParameters =>
				{
					applyParameters.SetCanEnter(false);
				}
			);
		}

		//ScenarioEvents.DuringAttackEvent.Subscribe(this,
		//	parameters => parameters.Performer is Character && parameters.AbilityState.SingleTargetRangeType == RangeType.Melee && parameters.AbilityState);
	}
}
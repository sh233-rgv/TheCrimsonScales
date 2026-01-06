using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class Scenario011 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario011.tscn";
	public override int ScenarioNumber => 11;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new CustomScenarioGoals("Kill all enemies to win this scenario.");

	public override string BGSPath => null;

	private List<Obstacle> _barrels;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO: Scenario Effect

		_barrels = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.GetHexObject<Obstacle>()).ToList();

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

		ScenarioEvents.DuringAttackEvent.Subscribe(this,
			parameters => parameters.Performer is Character && parameters.AbilityState.SingleTargetRangeType == RangeType.Melee &&
			              parameters.AbilityState.IsSingleTarget,
			async parameters =>
			{
				await parameters.AbilityState.SetPerformHex(hexes =>
				{
					hexes.AddRange(_barrels.Select(barrel => barrel.Hex));
				});
			}, EffectType.Selectable,
			effectButtonParameters: new IconEffectButton.Parameters("res://Art/OverlayTiles/Barrel 1h.png"),
			effectInfoViewParameters: new TextEffectInfoView.Parameters("Perform the attack as if you were occupying another barrel"));

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters => parameters.RoundNumber == 1,
			async parameters =>
			{
				foreach(Room room in GameController.Instance.Map.Rooms.Where(room => room.Figures.Any(figure => figure is Character)))
				{
					await SpawnMonster(null, ModelDB.Monster<InoxGuard>(), MonsterType.Normal, room.Hexes);
					await SpawnMonster(null, ModelDB.Monster<InoxArcher>(), MonsterType.Elite, room.Hexes);
				}

				ScenarioEvents.RoundEndedEvent.Unsubscribe(this);
			});
		
		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => GameController.Instance.Map.Figures.All(figure => figure.Alignment != Alignment.Enemies),
			async parameters =>
			{
				foreach(Room room in GameController.Instance.Map.Rooms.Where(room => room.Figures.Any(figure => figure is Character)))
				{
					await SpawnMonster(null, ModelDB.Monster<InoxGuard>(), MonsterType.Elite, room.Hexes);
					await SpawnMonster(null, ModelDB.Monster<InoxShaman>(), MonsterType.Elite, room.Hexes);
				}
				ScenarioEvents.FigureKilledEvent.Unsubscribe(this);

				((KillAlLEnemiesScenarioGoals)ScenarioGoals).EnemiesToBeSpawned = false;
				
				ScenarioEvents.FigureKilledEvent.Subscribe(this,
					_ => GameController.Instance.Map.Figures.All(figure => figure.Alignment != Alignment.Enemies),
					async _ =>
					{
						foreach(Room room in GameController.Instance.Map.Rooms.Where(room => room.Figures.Any(figure => figure is Character)))
						{
							await SpawnMonster(null, ModelDB.Monster<FlameDemon>(), MonsterType.Normal, room.Hexes);
							await SpawnMonster(null, ModelDB.Monster<NightDemon>(), MonsterType.Normal, room.Hexes);
						}
					});
			});
	}
}
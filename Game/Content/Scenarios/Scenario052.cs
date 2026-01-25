using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class Scenario052 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario052.tscn";
	public override int ScenarioNumber => 52;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<SideScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() =>
		new CustomScenarioGoals(
			$"Have both pressure plates {Icons.InlineMarker(Marker.Type.b)} be occupied at the end of any round to win this scenario.");

	private List<PressurePlate> _pressurePlatesA;
	private List<PressurePlate> _pressurePlatesB;
	private Hex _markerCHex;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO: Dark Pits cannot be moved

		_pressurePlatesA = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.GetHexObject<PressurePlate>()).ToList();
		_pressurePlatesB = GameController.Instance.Map.GetMarkers(Marker.Type.b).Select(marker => marker.GetHexObject<PressurePlate>()).ToList();
		_markerCHex = GameController.Instance.Map.GetMarker(Marker.Type.c).Hex;

		ScenarioCheckEvents.CanEnterCheckEvent.Subscribe(this,
			parameters =>
			{
				GD.Print(parameters.Hex.HasHexObjectOfType<DarkPitObstacle>());
				return parameters.Figure is Character or Summon && (parameters.Hex.MapTile == GameController.Instance.Map.Rooms[0].MapTiles[1] ||
				                                                    parameters.Hex.HasHexObjectOfType<DarkPitObstacle>());
			},
			parameters =>
			{
				parameters.SetCanEnter(false);
			});

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters => parameters.RoundNumber % 2 == 0,
			async parameters =>
			{
				await SpawnMonster(null, ModelDB.Monster<SpittingDrake>(), CalculateMonsterType(parameters.RoundNumber), _markerCHex);
			});

		ScenarioCheckEvents.SpawnCoinCheckEvent.Subscribe(this,
			parameters => parameters.Dropper is Monster monster && monster.MonsterModel is Ooze,
			parameters =>
			{
				parameters.SetCoinsToSpawn(2);
			});

		ScenarioEvents.FigureTurnEndedEvent.Subscribe(this, _pressurePlatesA,
			parameters => _pressurePlatesA.Select(pressurePlate => pressurePlate.Hex).Contains(parameters.Figure.Hex),
			async parameters =>
			{
				PressurePlate pressurePlate = parameters.Figure.Hex.GetHexObjectOfType<PressurePlate>();
				await pressurePlate.Destroy();
				_pressurePlatesA.Remove(pressurePlate);
				if(_pressurePlatesA.Any())
				{
					await PressurePlatesADestroyed();
				}
			});
	}

	private MonsterType CalculateMonsterType(int roundNumber)
	{
		if(GameController.Instance.SavedCampaign.Characters.Count > 3 ||
		   (roundNumber % 4 == 0 && GameController.Instance.SavedCampaign.Characters.Count == 3))
		{
			return MonsterType.Elite;
		}

		return MonsterType.Normal;
	}

	private async GDTask PressurePlatesADestroyed()
	{
		ScenarioCheckEvents.CanEnterCheckEvent.Unsubscribe(this);
		List<Hex> hexes = [];
		foreach(DarkPitObstacle darkPit in GameController.Instance.Map.GetChildrenOfType<DarkPitObstacle>())
		{
			hexes.AddRange(darkPit.Hexes);
			await darkPit.Destroy(forceDestroy: true);
		}

		foreach(Hex hex in hexes)
		{
			await AbilityCmd.CreateDifficultTerrain(hex,
				ResourceLoader.Load<PackedScene>("res://Content/OverlayTiles/DifficultTerrain/Water1H.tscn"));
		}

		ScenarioEvents.SufferDamageEvent.Subscribe(this,
			parameters => parameters.Figure is Character character && character.ObtainedCoins > 0 && parameters.WouldSufferDamage &&
			              RangeHelper.GetHexesInRange(character.Hex, 1).Any(hex => hex.HasHexObjectOfType<Water>()),
			async parameters =>
			{
				((Character)parameters.Figure).RemoveCoin();
				parameters.SetDamagePrevented();
				await GDTask.CompletedTask;
			}, EffectType.Selectable,
			effectButtonParameters: new IconEffectButton.Parameters("res://Art/Other/Coin.png"),
			effectInfoViewParameters: new TextEffectInfoView.Parameters(
				$"Return one money token to the supply to negate the {Icons.Inline(Icons.Damage)}"));

		ScenarioEvents.RoundEndedEvent.Subscribe(this, _pressurePlatesB,
			parameters => _pressurePlatesB.All(pressurePlate => pressurePlate.Hex.IsOccupied()),
			async parameters =>
			{
				await ((CustomScenarioGoals)ScenarioGoals).Win();
			});
	}
}
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public class Scenario003 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario003.tscn";
	public override int ScenarioNumber => 3;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [new ScenarioConnection<Scenario007>()];
	// public override IEnumerable<ScenarioConnection> Connections => [new ScenarioConnection<Scenario004>(), new ScenarioConnection<Scenario007>()];

	protected override ScenarioGoals CreateScenarioGoals() => new KillSpecificEnemyTypeGoals(ModelDB.Monster<HydraSpirit>(), "Kill the Hydra Spirit to win this scenario.");

	public override string BGMPath => "res://Audio/BGM/Dark-Abyss.ogg";
	public override string BGSPath => "res://Audio/BGS/Cave.ogg";

	private readonly List<Water> _waterTiles = new List<Water>();

	public override async GDTask Start()
	{
		await base.Start();

		UpdateScenarioText(
			$"At the end of each round, the water tiles marked {Icons.Marker(Marker.Type.a)} move one hex toward the hexes marked {Icons.Marker(Marker.Type.b)}. " +
			"These water tiles cannot be removed. After the first round, a new column of water tiles will spawn to the right of the first colum." +
			$"\nWhen all hexes marked {Icons.Marker(Marker.Type.b)} are occupied by water tiles, the scenario is immediately lost.");

		foreach(Marker marker in GameController.Instance.Map.Markers)
		{
			if(marker.MarkerType == Marker.Type.a)
			{
				_waterTiles.Add(marker.GetHexObject<Water>());
			}
		}

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters => true,
			async parameters =>
			{
				// Move all water tiles one hex to the left
				foreach(Water water in _waterTiles)
				{
					Hex currentHex = water.Hex;
					Hex newHex = GameController.Instance.Map.GetHex(currentHex.Coords.Add(Direction.West));
					water.TweenGlobalPosition(newHex.GlobalPosition, 0.5f).SetEasing(Easing.OutQuad).PlayFastForwardable();
				}

				await GDTask.DelayFastForwardable(0.5f);

				foreach(Water water in _waterTiles)
				{
					Hex currentHex = water.Hex;
					Hex newHex = GameController.Instance.Map.GetHex(currentHex.Coords.Add(Direction.West));
					water.SetOriginHexAndRotation(newHex);
				}

				if(parameters.RoundIndex == 0)
				{
					// Spawn a new column of water hexes to follow the first column
					for(int i = _waterTiles.Count - 1; i >= 0; i--)
					{
						Water water = _waterTiles[i];
						Hex currentHex = water.Hex;
						Hex newHex = GameController.Instance.Map.GetHex(currentHex.Coords.Add(Direction.East));
						Water newWater =
							await AbilityCmd.CreateDifficultTerrain(newHex,
								ResourceLoader.Load<PackedScene>("res://Content/OverlayTiles/DifficultTerrain/Water1H.tscn")) as Water;
						_waterTiles.Add(newWater);
					}

					UpdateScenarioText(
						$"At the end of each round, the water tiles marked {Icons.Marker(Marker.Type.a)} and the water tiles to the right of them move one hex toward the hexes marked {Icons.Marker(Marker.Type.b)}. " +
						"These water tiles cannot be removed." +
						$"\nWhen all hexes marked {Icons.Marker(Marker.Type.b)} are occupied by water tiles, the scenario is immediately lost.");
				}

				if(parameters.RoundIndex == 10)
				{
					// The scenario is lost, the water is all the way to the left
					await AbilityCmd.Lose();
				}
			});
	}
}
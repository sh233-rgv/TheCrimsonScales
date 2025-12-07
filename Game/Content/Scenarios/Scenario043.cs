using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class Scenario043 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario043.tscn";
	public override int ScenarioNumber => 43;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<EventScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillAlLEnemiesScenarioGoals();

	private Door _door1;
	private Door _door2;
	private Door _door3;
	private IEnumerable<Marker> _markersA;
	private IEnumerable<Marker> _markersB;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		GameController.Instance.Map.Treasures[0].SetItemLoot(ModelDB.Item<AshsteelGauntlets>());

		Marker marker1 = GameController.Instance.Map.GetMarker(Marker.Type._1);
		_door1 = marker1.GetHexObject<Door>();

		Marker marker2 = GameController.Instance.Map.GetMarker(Marker.Type._2);
		_door2 = marker2.GetHexObject<Door>();

		Marker marker3 = GameController.Instance.Map.GetMarker(Marker.Type._3);
		_door3 = marker3.GetHexObject<Door>();
		
		_markersA = GameController.Instance.Map.GetMarkers(Marker.Type.a);
		_markersB = GameController.Instance.Map.GetMarkers(Marker.Type.b);

		ScenarioEvents.AbilityStartedEvent.Subscribe(this,
			parameters => parameters.AbilityState is AttackAbility.State &&
				(parameters.Performer is Character || parameters.Performer is Summon) &&
				GameController.Instance.Map.Rooms[0].MapTiles.Contains(parameters.Performer.Hex.MapTile),
			async parameters =>
            {
				parameters.AbilityState.SetBlocked();

				await GDTask.CompletedTask;
            });

		ScenarioEvents.AbilityCardSideStartedEvent.Subscribe(this,
			parameters => !parameters.ForgoneAction && RangeHelper.GetFiguresInRange(parameters.Performer.Hex, 1)
								.Any(figure => figure.EnemiesWith(parameters.Performer)),
			async parameters =>
			{
				parameters.ForgoAction();

				ActionState actionState = new ActionState(parameters.Performer, [PushAbility.Builder()
					.WithPush(2)
					.WithRange(1)
					.Build()]);
				await actionState.Perform();
			},
			EffectType.Selectable,
			effectButtonParameters: new IconEffectButton.Parameters(Icons.Push),
			effectInfoViewParameters: new TextEffectInfoView.Parameters($"Perform {Icons.Inline(Icons.Push)}2, {Icons.Inline(Icons.Range)}1")
		);
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters parameters)
	{
		await base.OnRoomRevealed(parameters);

		if(parameters.OpenedDoor == _door1)
        {
			int houndsToSpawn = 0;
            ScenarioEvents.FigureKilledEvent.Subscribe(this, _door1,
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel.Name == "Hound" &&
					GameController.Instance.Map.Figures.Any(figure => figure is Monster monster && monster.MonsterModel is SlyWolf),
				async applyParameters =>
                {
                    await AbilityCmd.SufferDamage(null, GameController.Instance.Map.Figures
						.First(figure => figure is Monster monster && monster.MonsterModel is SlyWolf),
						GameController.Instance.SavedCampaign.Characters.Count);
					houndsToSpawn++;
                });

			ScenarioEvents.RoundEndedEvent.Subscribe(this, _door1,
				canApplyParameters => houndsToSpawn > 0,
				async applyParameters =>
                {
                    while(houndsToSpawn > 0)
                    {
						await SpawnMonster(null, ModelDB.Monster<Hound>(), MonsterType.Normal, _markersA.Select(marker => marker.Hex));
                        houndsToSpawn--;
                    }
                });
			
			ScenarioCheckEvents.CanEnterMapTileCheckEvent.Subscribe(this, _door1,
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel is Hound &&
					!GameController.Instance.Map.Rooms[1].MapTiles.Contains(canApplyParameters.MapTile),
				applyParameters =>
				{
					applyParameters.SetCanEnter(false);
				}
			);

			ScenarioCheckEvents.CanBeFocusedCheckEvent.Subscribe(this, _door1,
				canApplyParameters => canApplyParameters.Performer is Monster monster && monster.MonsterModel is Hound &&
					!GameController.Instance.Map.Rooms[1].MapTiles.Contains(canApplyParameters.PotentialTarget.Hex.MapTile),
				applyParameters =>
                {
                    applyParameters.SetCannotBeFocused();
                });
        }
		else if(parameters.OpenedDoor == _door2)
        {
			int caveBearsToSpawn = 0;
            ScenarioEvents.FigureKilledEvent.Subscribe(this, _door2,
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel.Name == "Cave Bear" &&
					GameController.Instance.Map.Figures.Any(figure => figure is Monster monster && monster.MonsterModel is GoringGrizzly),
				async applyParameters =>
                {
					Monster goringGrizzly = (Monster)GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is GoringGrizzly);
                    ((ShieldTrait)goringGrizzly.Stats.Traits.First(trait => trait is ShieldTrait)).ChangeShieldValue(goringGrizzly, -1);
					caveBearsToSpawn++;

					await GDTask.CompletedTask;
                });

			ScenarioEvents.RoundEndedEvent.Subscribe(this, _door2,
				canApplyParameters => caveBearsToSpawn > 0,
				async applyParameters =>
                {
                    while(caveBearsToSpawn > 0)
                    {
						await SpawnMonster(null, ModelDB.Monster<CaveBear>(), MonsterType.Normal, _markersB.Select(marker => marker.Hex));
                        caveBearsToSpawn--;
                    }
                });
			
			ScenarioCheckEvents.CanEnterMapTileCheckEvent.Subscribe(this, _door2,
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel is CaveBear &&
					!GameController.Instance.Map.Rooms[2].MapTiles.Contains(canApplyParameters.MapTile),
				applyParameters =>
				{
					applyParameters.SetCanEnter(false);
				}
			);

			ScenarioCheckEvents.CanBeFocusedCheckEvent.Subscribe(this, _door2,
				canApplyParameters => canApplyParameters.Performer is Monster monster && monster.MonsterModel is CaveBear &&
					!GameController.Instance.Map.Rooms[2].MapTiles.Contains(canApplyParameters.PotentialTarget.Hex.MapTile),
				applyParameters =>
                {
                    applyParameters.SetCannotBeFocused();
                });
        }
		else if(parameters.OpenedDoor == _door3)
        {
            ScenarioEvents.AfterSufferDamageEvent.Subscribe(this, _door3,
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel.Name == "Giant Viper" &&
					GameController.Instance.Map.Figures.Any(figure => figure is Monster monster && monster.MonsterModel is KingCobra),
				async applyParameters =>
                {
					Monster goringGrizzly = (Monster)GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is GoringGrizzly);
                    ((ShieldTrait)goringGrizzly.Stats.Traits.First(trait => trait is ShieldTrait)).ChangeShieldValue(goringGrizzly, -1);

					await AbilityCmd.SufferDamage(null, GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is KingCobra), applyParameters.Damage);

					await GDTask.CompletedTask;
                });

			ScenarioCheckEvents.CanEnterMapTileCheckEvent.Subscribe(this, _door3,
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel is GiantViper &&
					!GameController.Instance.Map.Rooms[3].MapTiles.Contains(canApplyParameters.MapTile),
				applyParameters =>
				{
					applyParameters.SetCanEnter(false);
				}
			);

			ScenarioCheckEvents.CanBeFocusedCheckEvent.Subscribe(this, _door3,
				canApplyParameters => canApplyParameters.Performer is Monster monster && monster.MonsterModel is GiantViper &&
					!GameController.Instance.Map.Rooms[3].MapTiles.Contains(canApplyParameters.PotentialTarget.Hex.MapTile),
				applyParameters =>
                {
                    applyParameters.SetCannotBeFocused();
                });
        }
	}

	private async GDTask Treasure33Loot(Character lootingCharacter)
    {
        lootingCharacter.SavedCharacter.AddGold(25);
		await AbilityCmd.AddCondition(null, lootingCharacter, Conditions.Poison1);
    }
}
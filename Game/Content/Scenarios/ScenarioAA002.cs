using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ScenarioAA002 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioAA002.tscn";
	public override string ScenarioPrefix => "AA";
	public override int ScenarioNumber => 2;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<AAScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() =>
		new KillSpecificEnemiesTypeGoals(ModelDB.Monster<Echo>(), "Kill the Echo to win this scenario.");

	private List<Door> _doors1;
	private List<Hex> _markerAHexes;
	private Hex _markerBHex;
	private Hex _markerCHex;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO: GameController.Instance.Map.Treasures[0].SetItemLoot(ModelDB.Item<DoomPowder>());

		_doors1 = GameController.Instance.Map.GetMarkers(Marker.Type._1).Select(marker => marker.GetHexObject<Door>()).ToList();
		_markerAHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex).ToList();
		_markerBHex = GameController.Instance.Map.GetMarker(Marker.Type.b).Hex;
		_markerCHex = GameController.Instance.Map.GetMarker(Marker.Type.c).Hex;

		//TODO: Scenario Effects

		UpdateScenarioText($"When any door {Icons.InlineMarker(Marker.Type._1)} is opened, open all doors {Icons.InlineMarker(Marker.Type._1)}.");
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters roomRevealedParameters)
	{
		await base.OnRoomRevealed(roomRevealedParameters);

		if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[1])
		{
			foreach(Door door in _doors1.Where(door => !door.Opened))
			{
				await door.Open(roomRevealedParameters.PotentialOpener);
			}

			UpdateScenarioText("");
		}
		else if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[2])
		{
			Figure echo = GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is Echo);

			ScenarioEvents.FigureTurnEndingEvent.Subscribe(this,
				parameters => parameters.Figure is Character && RangeHelper.GetFiguresInRange(parameters.Figure, 1)
					.Any(figure => figure is Monster monster && monster.MonsterModel is HarrowerInfester),
				async parameters =>
				{
					await new ActionState(parameters.Figure, [HealAbility.Builder().WithHealValue(1).WithTarget(Target.Self)]).Perform();
				});

			foreach(Character character in GameController.Instance.CharacterManager.Characters)
			{
				ScenarioEvents.DuringAttackEvent.Subscribe(this, character,
					parameters => parameters.Performer is Monster monster && monster.MonsterModel is HarrowerAegis or HarrowerInfester &&
					              !character.IsDead,
					async parameters =>
					{
						((Monster)parameters.Performer).SetAMDCardDeck(character.AMDCardDeck);
						await GDTask.CompletedTask;
					}, EffectType.Selectable,
					effectButtonParameters: new IconEffectButton.Parameters(character.ClassModel.IconPath),
					effectInfoViewParameters: new TextEffectInfoView.Parameters($"Use {character.DebugName}'s attack modifier deck"));
			}

			ScenarioCheckEvents.CanBeTargetedCheckEvent.Subscribe(this,
				parameters => parameters.PotentialTarget == echo && parameters.PotentialAbilityState is HealAbility.State,
				parameters =>
				{
					parameters.SetCannotBeTargeted();
				});

			ScenarioEvents.FigureKilledEvent.Subscribe(this,
				parameters => parameters.Figure is Monster monster && monster.MonsterModel is BlackImp && !echo.IsDead,
				async parameters =>
				{
					echo.SetHealth(echo.Health - 1);

					if(echo.Health == 0)
					{
						await AbilityCmd.KillOrExhaust(parameters.PotentialAbilityState, echo);
					}
				});

			ScenarioEvents.RoundStartBeforeCardSelectionEvent.Subscribe(this,
				_ => true,
				async _ =>
				{
					await SpawnMonster(null, ModelDB.Monster<BlackImp>(), MonsterType.Normal, _markerAHexes[0]);
					await SpawnMonster(null, ModelDB.Monster<BlackImp>(), MonsterType.Normal, _markerAHexes[1]);
					if(GameController.Instance.SavedCampaign.Characters.Count >= 3)
					{
						await SpawnMonster(null, ModelDB.Monster<BlackImp>(), MonsterType.Normal, _markerBHex);
					}

					if(GameController.Instance.SavedCampaign.Characters.Count >= 4)
					{
						await SpawnMonster(null, ModelDB.Monster<BlackImp>(), MonsterType.Normal, _markerCHex);
					}
				});

			ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this,
				parameters => parameters.Figure == echo,
				parameters =>
				{
					parameters.Add(new InfoTextExtraEffect.Parameters("This figure cannot be healed"));
				}
			);

			UpdateScenarioText($"""
			                    The Augurs Weave: The  Harrower  Aegis and  Harrower Infesters are allies to you and each other, and enemies to all other monster types. Characters that end their turn adjacent to at least one Harrower Infester perform {Icons.Inline(Icons.Heal)}1.

			                    When a Harrower Aegis or Harrower Infester attacks, they use the attack modifier deck you prefer.

			                    The Echo: The Living Spirit is The Echo and it has {GameController.Instance.SavedCampaign.Characters.Count * 6} hit points. It is immune to all damage and all negative conditions, cannot be healed, and always has {Icons.Inline(Icons.GetCondition(Conditions.Invisible))}. Each time a Black Imp dies, reduce the current hit points of The Echo by 1.

			                    Wounds of the Past: At the start of each round:
			                    Spawn one normal Black Imp at each {Icons.InlineMarker(Marker.Type.a)}{(GameController.Instance.SavedCampaign.Characters.Count >= 3 ? $"\nSpawn one normal Black Imp at {Icons.InlineMarker(Marker.Type.b)}" : "")}{(GameController.Instance.SavedCampaign.Characters.Count >= 4 ? $"\nSpawn one normal Black Imp at {Icons.InlineMarker(Marker.Type.c)}" : "")}
			                    """);
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class Scenario032 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario032.tscn";

	public override int ScenarioNumber => 32;
	public override string Name => "Confronting the Past";

	public override List<ScenarioLink> Links => [GloomhavenLink.Instance];

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();

	public override string IntroductionText =>
		"""
		You step into The Crimson Scale and pause. Something is different; there is a tension in the air, as if battle lines have been drawn.

		Arrok the barman, a loose group of other mercenaries and the singing Quatryl (of course) stand in a group facing you with Selandre at their head. You also notice Sankas cowering in the background.

		“Hello, friends” purrs Selandre disingenuously. “So glad you popped by…” “What’s going on?” you ask, confused. You came here for a drink, not a fight.

		“You still don’t get it, do you?” Selandre says, the smile gone now. “Do you really think I wanted you for your abilities?” she laughs, as do her various cronies. “I’ve spent the last six months trying to kill you off—though you have been very helpful to me.”

		On seeing your confusion, she carries on. “There were many threats to my domination of this town, and you’ve eliminated most of them, albeit mainly through good fortune and stupidity.”

		“Gaining the Orb of Embers and The Frosted Crystal for me, then using them to kill The Lavalite and the Icebound, was very helpful, as was disrupting the Aesther’s attempt to bio-engineer creatures to stop me. But best of all was your ‘rescue’ of my friend Sankas here—his weaponry skills have been most useful.” At this, one of Selandre’s henchmen wheels out a fearsome looking piece of artillery, glowing with the energy stone you rescued. Sankas looks distinctly ashamed, and scuttles off out of sight. Still completely taken aback by what you thought was a business arrangement, you only manage to utter “wha-why?”

		“Why?!” answers Selandre incredulously. “You mean you still don’t get it? Look around; you and your predecessors have been responsible for the slaughter of innocents, our friends, our… family!”

		And now you see it. Now you understand. Now you recognize the resemblance, even before Selandre shakes down her red hair. “Jekserah” you say, almost to yourself.

		“Don’t you mention my sister’s name!” Selandre screams in shock. “It isn’t just about her, although someone killed her, and I know you were associates of hers. We are The Crimson Scales, and we came together to wreak bloody justice for all the people you… self-appointed militia have taken from us. Now, we will have our REVENGE!”
		""";

	public override string ConclusionText =>
		"""
		Having fought for your life, you manage to overpower the last of Selandre’s horde. You have mixed feelings—it felt good to be in an elite group of mercenaries and now it turns out it was a trick all along. Still, as the last ones standing, you must be the elite, the best of the best—though your aching bodies disagree.

		Despite seemingly inheriting a bar of your own (which you now embarrassingly see was called The Crimson Scales all along), you know where you’re going. First you carefully pack the Frosted Crystal, The Orb of Embers and The Book of Naiqa and drop them at the Sanctuary, asking Athan Tredan to act as custodian of these precious, but dangerous artifacts.

		Then, you walk out of the door, and cross the road—there’s a dark corner of The Sleeping Lion with your name on it.
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		ModelDB.Monster<AncientArtillery>(),
		ModelDB.Monster<InoxArcher>(),
		ModelDB.Monster<InoxBodyguardScenario032>(),
		ModelDB.Monster<InoxGuard>(),
		ModelDB.Monster<LivingBones>(),
		ModelDB.Monster<Selandre>(),
	];

	public override List<SavedReward> Rewards =>
	[
	];

	private ScenarioRule _allEnemiesDeadRule;
	private ScenarioRule _drakeSpawnRule;
	private ScenarioRule _newSpawnRule;

	public Monster AncientArtillery { get; private set; }

	public override async GDTask InitializeAfterFirstRoomRevealed()
	{
		await base.InitializeAfterFirstRoomRevealed();

		await AddGoal(new KillSpecificEnemyTypeGoal(ModelDB.Monster<EternalDemon>()));

		int characterCount = GameController.Instance.CharacterManager.Characters.Count;

		List<Hex> aHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex).ToList();
		List<Hex> bHexes = GameController.Instance.Map.GetMarkers(Marker.Type.b).Select(marker => marker.Hex).ToList();
		List<Hex> allMarkedHexes = new List<Hex>();
		allMarkedHexes.AddRange(aHexes);
		allMarkedHexes.AddRange(bHexes);

		// List<MapTile> aMapTiles = aHexes.Select(hex => hex.MapTile).ToList();
		// List<MapTile> bMapTiles = bHexes.Select(hex => hex.MapTile).ToList();

		List<Hex> hexesToLink = new List<Hex>();
		foreach((Vector2I coords, Hex hex) in GameController.Instance.Map.Hexes)
		{
			if(RangeHelper.GetHexesInRange(hex, 1, false).Any(otherHex => otherHex.HasHexObjectOfType<DarkPitObstacle>()))
			{
				hexesToLink.Add(hex);
			}
		}

		foreach(Hex hex in hexesToLink)
		{
			foreach(Hex otherHex in hexesToLink)
			{
				if(hex.MapTile != otherHex.MapTile)
				{
					AbilityCmd.LinkHexes(hex, otherHex);
				}
			}
		}

		ScenarioCheckEvents.CanEnterCheckEvent.Subscribe(this,
			parameters => parameters.Hex.HasHexObjectOfType<DarkPitObstacle>(),
			parameters =>
			{
				parameters.SetCanEnter(false);
			}
		);

		ScenarioCheckEvents.CanBeFocusedCheckEvent.Subscribe(this,
			parameters =>
				parameters.Performer.Hex.MapTile != parameters.PotentialTarget?.Hex.MapTile,
			parameters =>
			{
				parameters.SetCannotBeFocused();
			}
		);

		ScenarioCheckEvents.CanBeTargetedCheckEvent.Subscribe(this,
			parameters =>
				parameters.Performer.Hex.MapTile != parameters.PotentialTarget?.Hex.MapTile,
			parameters =>
			{
				parameters.SetCannotBeTargeted();
			}
		);

		AddScenarioRule(textParameters =>
			$"""
			 The Dark Pit obstacles represent portals and cannot be destroyed. Hexes next to these portals are linked to other hexes next to portals on other map tiles, allowing characters to move between map tiles. Monsters cannot move onto map tiles unless otherwise stated in their abilities.
			 """);

		ScenarioRule teleportRule = AddScenarioRule(textParameters =>
			$"""
			 Figures can only interact with other figures if they are on the same map tile. Character and character summons cannot {Icons.Inline(Icons.Teleport, textParameters)} from one tile to another.
			 """);

		ScenarioCheckEvents.CanEnterCheckEvent.Subscribe(this, teleportRule,
			parameters => parameters.PotentialAbilityState is TeleportAbility.State,
			parameters =>
			{
				parameters.SetCanEnter(false);
			}
		);

// 		_drakeSpawnRule = AddScenarioRule(textParameters =>
// 			$"""
// 			 At the end of each round if there are not at least {characterCount} map tiles occupied by characters, spawn one normal Rending Drake on the hex marked with the letter {Icons.InlineMarker(Marker.Type.a, textParameters)} on any unoccupied tile. If at least one character occupies a tile with the letter {Icons.InlineMarker(Marker.Type.a, textParameters)}, instead spawn one normal Spitting Drake on the hex marked with the letter {Icons.InlineMarker(Marker.Type.b, textParameters)} on any unoccupied tile.
// 			 """);
		_drakeSpawnRule = AddScenarioRule(textParameters =>
			$"""
			 At the end of each round if there are not at least {characterCount} map tiles occupied by characters, spawn one normal Rending Drake on a hex marked {Icons.InlineMarker(Marker.Type.a, textParameters)} or {Icons.InlineMarker(Marker.Type.b, textParameters)} on any unoccupied tile.
			 """);

		_allEnemiesDeadRule = AddScenarioRule("Something will happen when all enemies are dead.");

		ScenarioEvents.RoundEndedEvent.Subscribe(this, _drakeSpawnRule,
			parameters =>
				GameController.Instance.CharacterManager.Characters.Select(character => character.Hex.MapTile).Distinct().Count() < characterCount,
			async parameters =>
			{
				// bool spawnTileA = true;
				// foreach(Character character in GameController.Instance.CharacterManager.Characters)
				// {
				// 	if(aMapTiles.Any(mapTile => character.Hex.MapTile == mapTile))
				// 	{
				// 		spawnTileA = false;
				// 		break;
				// 	}
				// }

				// IEnumerable<MapTile> unoccupiedMapTiles = GameController.Instance.Map.Rooms[0].MapTiles.Where(mapTile =>
				// 	GameController.Instance.CharacterManager.Characters.Any(character => character.Hex.MapTile != mapTile));
				IEnumerable<Hex> unoccupiedMapTileMarkerHexes = allMarkedHexes.Where(hex =>
					GameController.Instance.CharacterManager.Characters.All(character => character.Hex.MapTile != hex.MapTile));

				await SpawnMonster(null, ModelDB.Monster<RendingDrake>(), MonsterType.Normal, unoccupiedMapTileMarkerHexes);
			}
		);

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters =>
				KillAllEnemiesScenarioGoal.GetVisibleEnemyCount(false) == 0,
			async parameters =>
			{
				ScenarioEvents.FigureKilledEvent.Unsubscribe(this);

				//_allEnemiesDeadRule.Remove();

				_drakeSpawnRule.Remove();

				ScenarioEvents.RoundEndedEvent.Unsubscribe(this, _drakeSpawnRule);

				_newSpawnRule = AddScenarioRule(textParameters =>
					$"""
					 At the end of the round, spawn one {(characterCount == 4 ? "elite" : "normal")} Harrower Infester at each hex marked {Icons.InlineMarker(Marker.Type.a, textParameters)} and one {(characterCount >= 3 ? "elite" : "normal")} Living Spirit at each hex marked {Icons.InlineMarker(Marker.Type.b, textParameters)}.
					 """);

				ScenarioEvents.RoundEndedEvent.Subscribe(this,
					roundEndedParameters => true,
					async roundEndedParameters =>
					{
						ScenarioEvents.RoundEndedEvent.Unsubscribe(this);

						foreach(Hex aHex in aHexes)
						{
							await SpawnMonster(null, ModelDB.Monster<HarrowerInfester>(),
								characterCount == 4 ? MonsterType.Elite : MonsterType.Normal, aHex);
						}

						foreach(Hex bHex in bHexes)
						{
							await SpawnMonster(null, ModelDB.Monster<LivingSpirit>(),
								characterCount >= 3 ? MonsterType.Elite : MonsterType.Normal, bHex);
						}

						_newSpawnRule.Remove();

						ScenarioEvents.FigureKilledEvent.Subscribe(this,
							figureKilledParameters =>
								KillAllEnemiesScenarioGoal.GetVisibleEnemyCount(false) == 0,
							async figureKilledParameters =>
							{
								ScenarioEvents.FigureKilledEvent.Unsubscribe(this);

								_allEnemiesDeadRule.Remove();

								_newSpawnRule = AddScenarioRule(textParameters =>
									$"""
									 At the end of the round, the Eternal Demon will spawn at a marked hex on a map tile occupied by the least amount of characters.
									 """);

								ScenarioEvents.RoundEndedEvent.Subscribe(this,
									roundEndedParameters2 => true,
									async roundEndedParameters2 =>
									{
										ScenarioEvents.RoundEndedEvent.Unsubscribe(this);

										// List<Hex> lowestOccupyHexes = new List<Hex>();
										// int lowestCount = int.MaxValue;
										// foreach(Hex markedHex in allMarkedHexes)
										// {
										// 	int occupyCount = 0;
										// 	foreach(Character character in GameController.Instance.CharacterManager.Characters)
										// 	{
										// 		if(character.Hex.MapTile == markedHex.MapTile)
										// 		{
										// 			occupyCount++;
										// 		}
										// 	}
										//
										// 	if(occupyCount == lowestCount)
										// 	{
										// 		lowestOccupyHexes.Add(markedHex);
										// 	}
										// 	else if(occupyCount < lowestCount)
										// 	{
										// 		lowestOccupyHexes.Clear();
										// 		lowestOccupyHexes.Add(markedHex);
										// 		lowestCount = occupyCount;
										// 	}
										// }

										await SpawnMonster(null, ModelDB.Monster<EternalDemon>(), MonsterType.Boss, GetLeastOccupiedHexes());

										_newSpawnRule.Remove();
									}
								);

								await GDTask.CompletedTask;
							}
						);

						await GDTask.CompletedTask;
					}
				);

				await GDTask.CompletedTask;
			}
		);
	}

	public static List<Hex> GetLeastOccupiedHexes()
	{
		List<Hex> aHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex).ToList();
		List<Hex> bHexes = GameController.Instance.Map.GetMarkers(Marker.Type.b).Select(marker => marker.Hex).ToList();
		List<Hex> allMarkedHexes = new List<Hex>();
		allMarkedHexes.AddRange(aHexes);
		allMarkedHexes.AddRange(bHexes);

		List<Hex> lowestOccupiedHexes = new List<Hex>();
		int lowestCount = int.MaxValue;
		foreach(Hex markedHex in allMarkedHexes)
		{
			int occupyCount = 0;
			foreach(Character character in GameController.Instance.CharacterManager.Characters)
			{
				if(character.Hex.MapTile == markedHex.MapTile)
				{
					occupyCount++;
				}
			}

			if(occupyCount == lowestCount)
			{
				lowestOccupiedHexes.Add(markedHex);
			}
			else if(occupyCount < lowestCount)
			{
				lowestOccupiedHexes.Clear();
				lowestOccupiedHexes.Add(markedHex);
				lowestCount = occupyCount;
			}
		}

		return lowestOccupiedHexes;
	}
}
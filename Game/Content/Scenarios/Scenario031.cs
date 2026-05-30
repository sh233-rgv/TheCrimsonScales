using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class Scenario031 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario031.tscn";

	public override int ScenarioNumber => 31;
	public override string Name => "Eternal Portals";

	public override List<ScenarioLink> Links => [GloomhavenLink.Instance];

	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<WondrousScenarioChain>();

	public override string IntroductionText =>
		"""
		You are heading back to the Sleeping Lion for a nightcap, and laughing about the eccentric Brightspark, when you hear a loud, crackling, rending noise and a bright flash. A red rimmed hole opens up beneath you, sending you tumbling into a void, before the hole above you seals again.

		Once your eyes adjust to the gloom, you realize that you are each in small cell like rooms and are separated from each other by dark pits.

		As you start to figure out how you can escape, a voice booms through the cell structure. “You thought you could foil me, and rescue the Brightspark from my trap? Simple fools, prepare to feel the wrath of the Eternal Demon!”

		Another crackling noise, deafening in the small rooms, assaults your ears, and through multiple red voids, appears numerous drakes. Never a fan of them normally, you are extremely reluctant to have them as cell mates, and prepare to destroy them.
		""";

	public override string ConclusionText =>
		"""
		The Eternal Demon staggers backwards and falls, opening a dimensional gateway just before he hits the ground. As he is swallowed into the portal, it closes instantly, leaving the cells in total darkness.

		For a second you fumble completely blindly, until your vision slowly starts to adjust. As shapes become more distinct and solid, you realize you are back on the spot where you were first kidnapped from. Finding the whole experience deeply unsettling, you hurry to the Sleeping Lion, resolving to catch up with the Brightspark at the earliest opportunity.
		""";

	public override List<MonsterModel> MonsterModels { get; } =
	[
		ModelDB.Monster<EternalDemon>(),
		ModelDB.Monster<HarrowerInfester>(),
		ModelDB.Monster<LivingSpirit>(),
		ModelDB.Monster<RendingDrake>(),
		ModelDB.Monster<SpittingDrake>(),
	];

	public override List<SavedReward> Rewards =>
	[
		new GainCheckmarkReward()
	];

	private ScenarioRule _allEnemiesDeadRule;
	private ScenarioRule _drakeSpawnRule;
	private ScenarioRule _newSpawnRule;

	public override async GDTask StartOfScenarioEffects(Character character)
	{
		await AbilityCmd.AddCondition(null, character, Conditions.Immobilize);
	}

	public override async GDTask InitializeAfterFirstRoomRevealed()
	{
		await base.InitializeAfterFirstRoomRevealed();

		await AddGoal(new KillSpecificEnemyTypeGoal(ModelDB.Monster<EternalDemon>()));

		int characterCount = GameController.Instance.CharacterManager.Characters.Count;

		IEnumerable<Hex> aHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex);
		IEnumerable<Hex> bHexes = GameController.Instance.Map.GetMarkers(Marker.Type.b).Select(marker => marker.Hex);

		AddScenarioRule(textParameters =>
			$"""
			 The Dark Pit obstacles represent portals and cannot be destroyed. All portals are connected, allowing characters to move between map tiles. Monsters cannot move onto map tiles unless otherwise stated in their abilities.
			 """);

		AddScenarioRule(textParameters =>
			$"""
			 Figures can only interact with other figures if they are on the same map tile. Character and character summons cannot {Icons.Inline(Icons.Teleport, textParameters)} from one tile to another.
			 """);

		_drakeSpawnRule = AddScenarioRule(textParameters =>
			$"""
			 At the end of each round if there are not at least {characterCount} map tiles occupied by characters, spawn one normal Rending Drake on the hex marked with the letter {Icons.InlineMarker(Marker.Type.a, textParameters)} on any unoccupied tile. If at least one character occupies a tile with the letter {Icons.InlineMarker(Marker.Type.a, textParameters)}, instead spawn one normal Spitting Drake on the hex marked with the letter {Icons.InlineMarker(Marker.Type.b, textParameters)} on any unoccupied tile.
			 """);

		_allEnemiesDeadRule = AddScenarioRule("Something will happen when all enemies are dead.");

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters =>
				KillAllEnemiesScenarioGoal.GetVisibleEnemyCount(false) == 0,
			async parameters =>
			{
				ScenarioEvents.FigureKilledEvent.Unsubscribe(this);

				//_allEnemiesDeadRule.Remove();

				_newSpawnRule = AddScenarioRule(textParameters =>
					$"""
					 At the end of the round, spawn one {(characterCount == 4 ? "elite" : "normal")} Harrower Infester at each hex marked {Icons.InlineMarker(Marker.Type.a, textParameters)} and one {(characterCount >= 3 ? "elite" : "normal")} Living Spirit next to each hex marked {Icons.InlineMarker(Marker.Type.b, textParameters)}.
					 """);

				ScenarioEvents.RoundEndedEvent.Subscribe(this,
					roundEndedParameters => true,
					async roundEndedParameters =>
					{
						ScenarioEvents.RoundEndedEvent.Unsubscribe(this);

						//TODO: Spawn
						//await SpawnMonster(null, ModelDB.Monster<InoxBodyguard>(), MonsterType.Boss, _markerDHex);

						_newSpawnRule.Remove();

						ScenarioEvents.FigureKilledEvent.Subscribe(this,
							figureKilledParameters =>
								KillAllEnemiesScenarioGoal.GetVisibleEnemyCount(false) == 0,
							async figureKilledParameters =>
							{
								ScenarioEvents.FigureKilledEvent.Unsubscribe(this);

								_newSpawnRule = AddScenarioRule(textParameters =>
									$"""
									 At the end of the round, the Eternal Demon will spawn on a map tile occupied by the least amount of characters.
									 """);

								ScenarioEvents.RoundEndedEvent.Subscribe(this,
									roundEndedParameters2 => true,
									async roundEndedParameters2 =>
									{
										ScenarioEvents.RoundEndedEvent.Unsubscribe(this);

										//TODO: Spawn
										//await SpawnMonster(null, ModelDB.Monster<InoxBodyguard>(), MonsterType.Boss, _markerDHex);

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
}
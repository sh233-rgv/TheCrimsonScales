using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ScenarioAA001 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioAA001.tscn";
	public override int ScenarioNumber => 1;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<AAScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() =>
		new CustomScenarioGoals("Protect and the Harrower Aegis and kill the Gilded One to complete this scenario.");

	protected override List<MonsterModel> SpawnedMonsterModels { get; } =
	[
		ModelDB.Monster<BanditArcher>(), ModelDB.Monster<BanditGuard>(), ModelDB.Monster<GildedOne>(), ModelDB.Monster<WindDemon>(),
		ModelDB.Monster<FrostDemon>()
	];

	private Monster _aegis;
	private List<Hex> _markerAHexes;
	private readonly List<MonsterModel> _monsterSpawnOrder =
	[
		ModelDB.Monster<BanditGuard>(), ModelDB.Monster<BanditArcher>(), ModelDB.Monster<Hound>(), ModelDB.Monster<WindDemon>(),
		ModelDB.Monster<BanditGuard>(), ModelDB.Monster<BanditArcher>(), ModelDB.Monster<Hound>(), ModelDB.Monster<FrostDemon>()
	];

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO: Treasure: 50 Collective Gold

		_aegis = (Monster)GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is HarrowerAegis);
		_markerAHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex).ToList();

		UpdateScenarioText($"""
		                    The elite Harrower Aegis is an ally to you and an enemy to all monster types. If the Aegis is killed, the scenario is lost.

		                    At the end of each round, the Harrower Aegis performs {Icons.Inline(Icons.Move)}3, controlled by a consensus among all players. When the Harrower Aegis attacks, they use the attack modifier deck you prefer. Any character may lose one card from their hand negate source of {Icons.Inline(Icons.Damage)} the Harrower Aegis suffers.

		                    Stairs cannot be destroyed or removed.

		                    The Aureate Claw: At the start of each round, spawn the following enemies:
		                    2nd: {GameController.Instance.SavedCampaign.Characters.Count} normal Bandit Guards
		                    3rd: {GameController.Instance.SavedCampaign.Characters.Count} normal Bandit Archers
		                    4th: {GameController.Instance.SavedCampaign.Characters.Count} normal Hounds
		                    5th: {GameController.Instance.SavedCampaign.Characters.Count} normal Wind Demons
		                    6th: {GameController.Instance.SavedCampaign.Characters.Count} normal Bandit Guards
		                    7th: {GameController.Instance.SavedCampaign.Characters.Count} normal Bandit Archers
		                    8th: {GameController.Instance.SavedCampaign.Characters.Count} normal Hounds
		                    9th: {GameController.Instance.SavedCampaign.Characters.Count} normal Frost Demons

		                    When spawning enemies, spawn the first enemy at the lettered hex closest to the Harrower Aegis, with the next enemy spawning at the  next closest lettered hex. Follow this pattern for all spawned enemies.

		                    Something will happen at the end of the ninth round.
		                    """);

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => parameters.Figure == _aegis,
			async _ =>
			{
				await ((CustomScenarioGoals)ScenarioGoals).Lose();
			});

		ScenarioEvents.RoundEndedEvent.Subscribe(this, _aegis,
			_ => true,
			async _ =>
			{
				await new ActionState(null, _aegis, GameController.Instance.CharacterManager.FirstAlive(),
					[MoveAbility.Builder().WithDistance(3)]).Perform();
			});

		foreach(Character character in GameController.Instance.CharacterManager.Characters)
		{
			ScenarioEvents.DuringAttackEvent.Subscribe(this, character,
				parameters => parameters.Performer == _aegis && !character.IsDead,
				async _ =>
				{
					_aegis.SetAMDCardDeck(character.AMDCardDeck);
					await GDTask.CompletedTask;
				}, EffectType.Selectable,
				effectButtonParameters: new IconEffectButton.Parameters(character.ClassModel.IconPath),
				effectInfoViewParameters: new TextEffectInfoView.Parameters($"Use {character.DebugName}'s attack modifier deck"));

			ScenarioEvents.SufferDamageEvent.Subscribe(this,
				parameters => parameters.Figure == _aegis && parameters.WouldSufferDamage && !character.IsDead &&
				              character.Cards.Any(card => card.CardState == CardState.Hand && card.OriginalOwner == character),
				async parameters =>
				{
					AbilityCard card = await AbilityCmd.SelectAbilityCard(character, CardState.Hand, true, card => card.OriginalOwner == character,
						hintText: "Select a card to lose");
					await AbilityCmd.LoseCard(card);

					parameters.SetDamagePrevented();
				}, EffectType.Selectable,
				effectButtonParameters: new IconEffectButton.Parameters(Icons.LoseCard),
				effectInfoViewParameters: new TextEffectInfoView.Parameters($"Lose a card from {character.DebugName}'s hand to negate the damage"));
		}

		ScenarioEvents.RoundStartBeforeCardSelectionEvent.Subscribe(this,
			parameters => parameters.RoundNumber <= 9 && parameters.RoundNumber != 1,
			async parameters =>
			{
				List<Hex> spawnHexes = [];
				List<(Hex, int)> distances = _markerAHexes
					.Select(hex => (hex, RangeHelper.Distance(_aegis.Hex, hex)))
					.OrderBy(t => t.Item2)
					.ToList();
				for(int i = 0; i < GameController.Instance.SavedCampaign.Characters.Count; i++)
				{
					if(!spawnHexes.Any())
					{
						spawnHexes.Add(distances[0].Item1);
						int minDistance = distances[0].Item2;
						distances.RemoveAt(0);
						foreach((Hex hex, int dist) in distances)
						{
							if(dist == minDistance)
							{
								spawnHexes.Add(hex);
							}
							else
							{
								break;
							}
						}

						distances.RemoveRange(0, spawnHexes.Count - 1);
					}

					Hex spawnHex;
					if(spawnHexes.Count > GameController.Instance.SavedCampaign.Characters.Count - i)
					{
						spawnHex = await AbilityCmd.SelectHex(GameController.Instance.CharacterManager.FirstAlive(),
							hexes => hexes.AddRange(spawnHexes), true, "Select a marker to spawn from");
					}
					else
					{
						spawnHex = spawnHexes[0];
					}

					await SpawnMonster(null, _monsterSpawnOrder[parameters.RoundNumber - 2], MonsterType.Normal, spawnHex);
					spawnHexes.Remove(spawnHex);
				}
			});

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters => parameters.RoundNumber == 9,
			async _ =>
			{
				List<Hex> spawnHexes = [];
				List<(Hex, int)> distances = _markerAHexes
					.Select(hex => (hex, RangeHelper.Distance(_aegis.Hex, hex)))
					.OrderBy(t => t.Item2)
					.ToList();
				int distance = distances[1].Item2;
				foreach((Hex hex, int dist) in distances)
				{
					if(dist == distance)
					{
						spawnHexes.Add(hex);
					}
				}

				Hex spawnHex;
				if(spawnHexes.Count > 1)
				{
					spawnHex = await AbilityCmd.SelectHex(GameController.Instance.CharacterManager.FirstAlive(),
						hexes => hexes.AddRange(spawnHexes), true, "Select a marker to spawn the Gilded One from");
				}
				else
				{
					spawnHex = spawnHexes[0];
				}

				Monster gildedOne = await SpawnMonster(null, ModelDB.Monster<GildedOne>(), MonsterType.Named, spawnHex,
					monsterLevel: GameController.Instance.SavedScenario.ScenarioLevel + 1);
				ScenarioEvents.RoundEndedEvent.Unsubscribe(this);

				ScenarioEvents.RoundEndedEvent.Subscribe(this,
					_ => gildedOne.IsDead,
					async _ =>
					{
						await ((CustomScenarioGoals)ScenarioGoals).Win();
					});
			});
	}
}
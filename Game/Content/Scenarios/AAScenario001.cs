using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class AAScenario001 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/AAScenario001.tscn";
	public override int ScenarioNumber => 54;
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

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => parameters.Figure == _aegis,
			async _ =>
			{
				await ((CustomScenarioGoals)ScenarioGoals).Lose();
			});

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			_ => true,
			async _ =>
			{
				await new ActionState(null, _aegis, GameController.Instance.CharacterManager.FirstAlive(),
					[MoveAbility.Builder().WithDistance(3).Build()]).Perform();
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
						foreach((Hex, int) markerDistance in distances)
						{
							if()
						}
					}
				}
			});
	}
}
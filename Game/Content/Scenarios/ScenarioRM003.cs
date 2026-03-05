using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ScenarioRM003 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioRM003.tscn";
	public override string ScenarioPrefix => "RM";
	public override int ScenarioNumber => 3;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<RMScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillSpecificEnemiesTypeGoals(ModelDB.Monster<GoremyonShatterMind>(),
		"Kill the Goremyon Shatter-Mind to win this scenario.");


	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => parameters.PotentialKiller is Character && parameters.PotentialKiller.EnemiesWith(parameters.Figure),
			async parameters =>
			{
				//TODO: Scenario Effects Check
				await AbilityCmd.AddConditions(null, parameters.PotentialKiller, [Conditions.Muddle, Conditions.Curse]);
			});

		ScenarioEvents.InflictConditionEvent.Subscribe(this,
			parameters =>
				parameters.Target.Alignment is Alignment.Enemies &&
				(AbilityCmd.CheckImmunity(parameters.ConditionModel, Conditions.Immobilize) ||
				 AbilityCmd.CheckImmunity(parameters.ConditionModel, Conditions.Disarm) ||
				 AbilityCmd.CheckImmunity(parameters.ConditionModel, Conditions.Stun)),
			async parameters =>
			{
				parameters.SetPrevented(true);

				await GDTask.CompletedTask;
			}
		);

		ScenarioCheckEvents.ImmunitiesVisualCheckEvent.Subscribe(this,
			parameters => parameters.Figure.Alignment is Alignment.Enemies,
			parameters =>
			{
				parameters.AddImmunity(Conditions.Immobilize);
				parameters.AddImmunity(Conditions.Disarm);
				parameters.AddImmunity(Conditions.Stun);
			}
		);

		List<Hex> markerAHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex).ToList();
		Hex markerAHexRoom0 = markerAHexes.First(hex => hex.GetRoom() == GameController.Instance.Map.Rooms[0]);
		markerAHexes.Remove(markerAHexRoom0);
		foreach(Hex hex in markerAHexes)
		{
			AbilityCmd.LinkHexes(markerAHexRoom0, hex);
			hex.Reveal();
		}

		ScenarioEvents.FigureEnteredHexEvent.Subscribe(this,
			parameters => markerAHexes.Contains(parameters.Hex),
			async parameters =>
			{
				await GameController.Instance.Map.Rooms[1].Reveal(null, parameters.Figure, false);
				ScenarioEvents.FigureEnteredHexEvent.Unsubscribe(this);
			});

		UpdateScenarioText(
			$"Side Passage: Stairs {Icons.InlineMarker(Marker.Type.a)} lead further into Goremyon's mansion. Non-adjacent hexes {Icons.InlineMarker(Marker.Type.a)} are linked.");
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters roomRevealedParameters)
	{
		await base.OnRoomRevealed(roomRevealedParameters);

		if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[1])
		{
			List<Hex> markerBHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex).ToList();
			Hex markerBHexRoom1 = markerBHexes.First(hex => hex.GetRoom() == GameController.Instance.Map.Rooms[1]);
			markerBHexes.Remove(markerBHexRoom1);
			foreach(Hex hex in markerBHexes)
			{
				AbilityCmd.LinkHexes(markerBHexRoom1, hex);
				hex.Reveal();
			}

			UpdateScenarioText(
				$"Corner Staircase: Stairs {Icons.InlineMarker(Marker.Type.b)} lead further into the mansion. Non-adjacent hexes {Icons.InlineMarker(Marker.Type.b)} are linked.");
		}
		else if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[2])
		{
			Figure goremyonShatterMind =
				GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is GoremyonShatterMind);
			Monster inoxBloodguard =
				(Monster)GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is InoxBloodguard);

			ScenarioEvents.RoundStartBeforeCardSelectionEvent.Subscribe(this,
				_ => true,
				async _ =>
				{
					await StartRoundSpawn<BanditGuard>(GameController.Instance.Map.GetMarker(Marker.Type.c).Hex);
					await StartRoundSpawn<BanditArcher>(GameController.Instance.Map.GetMarker(Marker.Type.d).Hex);
					await StartRoundSpawn<InoxGuard>(GameController.Instance.Map.GetMarker(Marker.Type.e).Hex, 3);
					await StartRoundSpawn<InoxArcher>(GameController.Instance.Map.GetMarker(Marker.Type.f).Hex, 4);
				});

			//TODO: Draw two boss cards, one for each boss

			ScenarioEvents.FigureKilledEvent.Subscribe(this, new object(),
				parameters => parameters.Figure is Monster monster && monster.MonsterType is MonsterType.Normal &&
				              parameters.PotentialKiller != goremyonShatterMind && !goremyonShatterMind.IsDead,
				async _ =>
				{
					await AbilityCmd.SufferDamage(goremyonShatterMind, ScenarioLevel + 1, null);
				});

			ScenarioEvents.JustBeforeSufferDamageEvent.Subscribe(this,
				parameters => parameters.Figure == goremyonShatterMind && parameters.Damage >= goremyonShatterMind.Health,
				async parameters =>
				{
					parameters.AdjustDamage(goremyonShatterMind.Health);
					await GDTask.CompletedTask;
				});

			ScenarioEvents.RoundEndedEvent.Subscribe(this,
				_ => goremyonShatterMind.Health == 1,
				async _ =>
				{
					await AbilityCmd.KillOrExhaust(goremyonShatterMind, null);
				}, order: -1);

			ScenarioEvents.SufferDamageEvent.Subscribe(this,
				parameters => parameters.Figure == goremyonShatterMind && !inoxBloodguard.IsDead && parameters.FromAttack,
				async parameters =>
				{
					parameters.AddAdjustFinalDamage(damage => (damage + 1) / 2);
					await GDTask.CompletedTask;
				},
				EffectType.MandatoryBeforeOptionals, 100);

			ScenarioEvents.FigureTurnEndedEvent.Subscribe(this,
				parameters => parameters.Figure == inoxBloodguard && inoxBloodguard.Hex.GetRoom() != GameController.Instance.Map.Rooms[2],
				async _ =>
				{
					ActionState actionState = new ActionState(inoxBloodguard,
						[MonsterAbilityCardModel.MoveAbility(inoxBloodguard, +3).WithMoveType(MoveType.Jump)]);
					ScenarioCheckEvents.FigureFocusCheckEvent.Subscribe(this,
						parameters => parameters.ActionState == actionState,
						parameters =>
						{
							parameters.SetFocusFigure(goremyonShatterMind);
						});
					await actionState.Perform();
					ScenarioCheckEvents.FigureFocusCheckEvent.Unsubscribe(this);
				});

			UpdateScenarioText($"""
			                    Many as One: At the start of each round:
			                    If there are no Bandit Gaurds, spawn one normal Bandit Guard at hex {Icons.InlineMarker(Marker.Type.c)}. If there are no Bandit Archers, spawn one normal Bandit Archer at hex {Icons.InlineMarker(Marker.Type.d)}.{(CharacterCount >= 3 ? $"\n\nIf there are no Inox Guards, spawn one normal Inox Guard at hex {Icons.InlineMarker(Marker.Type.e)}." : "")}{(CharacterCount >= 4 ? $"\n\nIf there are no Inox Archers, spawn one normal Inox Archer at hex {Icons.InlineMarker(Marker.Type.f)}." : "")}

			                    The Sinking Kingpin: The Goremyon Shatter-Mind and Inox Bloodguard draw two separate cards from the boss ability deck each round.

			                    Whenever a normal enemy is killed by any source other than Goremyon’s Cranium Overload, Goremyon suﬀers {ScenarioLevel + 1} damage.

			                    Goremyon Shatter-Mind cannot be reduced below 1 hit point.

			                    If the Inox Bloodguard ends their turn outside the Great Hall (tiles I1A and I2A), they immediately perform {Icons.Inline(Icons.Move)}{inoxBloodguard.Stats.Move + 3}, {Icons.Inline(Icons.Jump)}
			                    """);
		}
	}

	private async GDTask StartRoundSpawn<T>(Hex spawnHex, int minCharacterCount = 0) where T : MonsterModel
	{
		if(GameController.Instance.SavedCampaign.Characters.Count >= minCharacterCount &&
		   GameController.Instance.Map.Figures.Any(figure => figure is Monster monster && monster.MonsterModel is T))
		{
			await SpawnMonster(null, ModelDB.Monster<T>(), MonsterType.Normal, spawnHex);
		}
	}

	protected override void UpdateScenarioText(string text)
	{
		base.UpdateScenarioText($"""
		                         Pervasive Mind: Whenever a character kills an enemy, that character gains {Icons.Inline(Icons.GetCondition(Conditions.Muddle))} and {Icons.Inline(Icons.GetCondition(Conditions.Curse))} as a scenario effect. 

		                         Parasitic Influence: All enemies are immune to {Icons.Inline(Icons.GetCondition(Conditions.Immobilize))}, {Icons.Inline(Icons.GetCondition(Conditions.Disarm))}, and {Icons.Inline(Icons.GetCondition(Conditions.Stun))}.


		                         """ + text);
	}
}
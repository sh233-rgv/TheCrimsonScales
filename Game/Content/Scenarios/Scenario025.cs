using Fractural.Tasks;

public class Scenario025 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario025.tscn";
	public override int ScenarioNumber => 25;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<WondrousScenarioChain>();
	//public override IEnumerable<ScenarioConnection> Connections => [new ScenarioConnection<Scenario031>()];

	protected override ScenarioGoals CreateScenarioGoals() => new CustomScenarioGoals("");

	private int _treasuresLooted;
	private Hex _markerAHex;

	public override async GDTask InitializeAfterFirstRoomRevealed()
	{
		await base.InitializeAfterFirstRoomRevealed();

		_markerAHex = GameController.Instance.Map.GetMarker(Marker.Type.a).Hex;

		NPC brightspark = await SpawnNPC(GameController.Instance.Map.GetMarker(Marker.Type.b).Hex, CharacterCount + ScenarioLevel * 3, "Brightspark",
			"res://Content/Scenarios/NPCs/Brightspark", 50, [
				MoveAbility.Builder().WithDistance(2).Build(),
				AttackAbility.Builder().WithDamage(1).Build()
			], $"{Icons.Inline(Icons.Move)}2\n{Icons.Inline(Icons.Attack)}1");

		foreach(Treasure treasure in GameController.Instance.Map.Treasures)
		{
			treasure.SetObtainLootFunction(async _ =>
			{
				switch(_treasuresLooted)
				{
					case 0:
						ScenarioEvents.AbilityStartedEvent.Subscribe(this, treasure,
							parameters => parameters.AbilityState is MoveAbility.State && parameters.Performer == brightspark,
							async parameters =>
							{
								((MoveAbility.State)parameters.AbilityState).AdjustMoveValue(1);
								await GDTask.CompletedTask;
							});
						ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this, treasure,
							parameters => parameters.Figure == brightspark,
							parameters =>
							{
								parameters.Add(new InfoTextExtraEffect.Parameters($"Add +1{Icons.Inline(Icons.Move)} to all moves"));
							});
						break;
					case 1:
						ScenarioEvents.AbilityStartedEvent.Subscribe(this, treasure,
							parameters => parameters.AbilityState is AttackAbility.State && parameters.Performer == brightspark,
							async parameters =>
							{
								((AttackAbility.State)parameters.AbilityState).AbilityAdjustAttackValue(1);
								await GDTask.CompletedTask;
							});
						ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this, treasure,
							parameters => parameters.Figure == brightspark,
							parameters =>
							{
								parameters.Add(new InfoTextExtraEffect.Parameters($"Add +1{Icons.Inline(Icons.Attack)} to all attacks"));
							});
						break;
					case 2:
						ScenarioEvents.AbilityStartedEvent.Subscribe(this, treasure,
							parameters => parameters.AbilityState is MoveAbility.State && parameters.Performer == brightspark,
							async parameters =>
							{
								((MoveAbility.State)parameters.AbilityState).AddJump();
								await GDTask.CompletedTask;
							});
						ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this, treasure,
							parameters => parameters.Figure == brightspark,
							parameters =>
							{
								parameters.Add(new InfoTextExtraEffect.Parameters($"Add {Icons.Inline(Icons.Jump)} to all moves"));
							});
						break;
				}

				_treasuresLooted++;
				UpdateScenarioText();

				await GDTask.CompletedTask;
			});
		}

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => parameters.Figure == brightspark,
			async _ =>
			{
				await AbilityCmd.Lose();
			}
		);

		ScenarioEvents.FigureTurnEndingEvent.Subscribe(this,
			ScenarioEvents.FigureTurnEnding.Subscription.ConsumeWildElement(
				parameters => parameters.Figure == brightspark,
				async _ =>
				{
					await new ActionState(brightspark, [HealAbility.Builder().WithHealValue(2).WithTarget(Target.Self).Build()]).Perform();
				}, effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.Heal)}2, self")));

		ScenarioEvents.FigureFoundFocusEvent.Subscribe(this,
			parameters =>
				parameters.Performer == brightspark &&
				parameters.AbilityState is MoveAbility.State &&
				parameters.Focus == null,
			async parameters =>
			{
				parameters.SetFocusHex(_markerAHex);

				ScenarioCheckEvents.AIMoveParametersCheckEvent.Subscribe(this,
					moveParameters => moveParameters.Performer == brightspark,
					moveParameters =>
					{
						moveParameters.SetRange(0);
						moveParameters.SetRangeType(RangeType.Melee);
						moveParameters.SetTargets(1);
						moveParameters.SetAOEPattern(null);
					}
				);

				ScenarioEvents.AbilityEndedEvent.Subscribe(this,
					abilityEndedParameters => abilityEndedParameters.Performer == brightspark,
					async _ =>
					{
						ScenarioEvents.AbilityEndedEvent.Unsubscribe(this);
						ScenarioCheckEvents.AIMoveParametersCheckEvent.Unsubscribe(this);

						await GDTask.CompletedTask;
					}
				);

				await GDTask.CompletedTask;
			});

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			_ => _treasuresLooted >= 4,
			async _ =>
			{
				await ((CustomScenarioGoals)ScenarioGoals).Win();
			});

		foreach(Character character in GameController.Instance.CharacterManager.Characters)
		{
			ScenarioEvents.DuringAttackEvent.Subscribe(this, character,
				parameters => parameters.Performer == brightspark && !character.IsDead,
				async _ =>
				{
					brightspark.SetAMDCardDeck(character.AMDCardDeck);
					await GDTask.CompletedTask;
				}, EffectType.Selectable,
				effectButtonParameters: new IconEffectButton.Parameters(character.ClassModel.IconPath),
				effectInfoViewParameters: new TextEffectInfoView.Parameters($"Use {character.DebugName}'s attack modifier deck"));
		}

		UpdateScenarioText();
	}

	private void UpdateScenarioText()
	{
		string text =
			$"""
			 Loot {4 - _treasuresLooted} more Goal treasure tiles and keep the Brightspark alive to win this scenario.

			 The Brightspark acts on Initiative 50 every turn, performing “{Icons.Inline(Icons.Move)}2, {Icons.Inline(Icons.Attack)}1” (using whichever modifier deck you prefer). For each Goal treasure tile you loot, the Brightspark gains the following benefit:
			 First tile: Add +1{Icons.Inline(Icons.Move)} to all moves
			 Second tile: Add +1{Icons.Inline(Icons.Attack)} to all attacks
			 Third tile: Add {Icons.Inline(Icons.Jump)} to all moves

			 Additionally, the Brightspark can consume {Icons.Inline(Icons.WildElement)} at the end of its turn to perform {Icons.Inline(Icons.Heal)}2, self. This is optional and players determine if this is performed.

			 Whenever there are no monsters on the map, the Brightspark will move toward hex {Icons.InlineMarker(Marker.Type.a)}.

			 If the Brightspark is killed, the scenario is immediately lost.
			 """;
		GameController.Instance.SpecialRulesView.SetText(text);
	}
}
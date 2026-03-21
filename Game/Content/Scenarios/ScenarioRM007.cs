using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class ScenarioRM007 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioRM007.tscn";
	public override string ScenarioPrefix => "RM";
	public override int ScenarioNumber => 7;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<RMScenarioChain>();

	private int _remainingEmpowerCount = 12;

	protected override ScenarioGoals CreateScenarioGoals() =>
		new KillSpecificEnemiesTypeGoals(ModelDB.Monster<RuinmawBossRoom5>(), "Kill the Ruinmaw to win this scenario.");

	private Door _door1;
	private List<Door> _doors2;
	private Door _door3;
	private Door _door4;
	private int _healValue;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		//TODO: Edit deck to have Ruinmaw AMDs (requires AMDs)

		_door1 = GameController.Instance.Map.GetMarker(Marker.Type._1).GetHexObject<Door>();
		_doors2 = GameController.Instance.Map.GetMarkers(Marker.Type._2).ConvertAll(marker => marker.GetHexObject<Door>());
		_door3 = GameController.Instance.Map.GetMarker(Marker.Type._3).GetHexObject<Door>();
		_door4 = GameController.Instance.Map.GetMarker(Marker.Type._4).GetHexObject<Door>();
		_healValue = CharacterCount +
		             ((Monster)GameController.Instance.Map.Figures.First(figure =>
			             figure is Monster monster && monster.MonsterModel is RuinmawBossRoom1)).Stats.Attack;

		ScenarioEvents.JustBeforeSufferDamageEvent.Subscribe(this,
			parameters => parameters.Figure is Monster monster && monster.MonsterModel is RuinmawBossRoom1 or RuinmawBossRoom3 &&
			              parameters.Damage >= monster.Health,
			async parameters =>
			{
				parameters.AdjustDamage(parameters.Figure.Health - 1);
				await GDTask.CompletedTask;
			});

		//TODO: ScenarioEvents.BeforeFigureKilled, for ruinmawbossroom1/3, can't be killed (requires something or the other, forget which one that is in)

		ScenarioEvents.AfterSufferDamageEvent.Subscribe(this,
			parameters => parameters.Figure is Monster monster && monster.MonsterModel is RuinmawBossRoom1 or RuinmawBossRoom3 &&
			              monster.Health == 1,
			async parameters =>
			{
				await parameters.Figure.Destroy();
			});

		ScenarioCheckEvents.SpawnCoinCheckEvent.Subscribe(this,
			parameters => parameters.Dropper is Monster monster && monster.MonsterModel is RuinmawBossRoom1 or RuinmawBossRoom3,
			parameters =>
			{
				parameters.SetCoinsToSpawn(0);
			});

		ScenarioCheckEvents.CanTargetInvisibleCheckEvent.Subscribe(this,
			parameters => parameters.Figure is Monster monster && monster.MonsterModel is RuinmawBoss,
			parameters =>
			{
				parameters.SetCanTargetInvisible();
			});

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			_ => !GameController.Instance.Map.Figures.Any(figure => figure is Monster monster && monster.MonsterModel is RuinmawBossRoom1),
			async _ =>
			{
				await _door1.Open(null);
				ScenarioEvents.RoundEndedEvent.Unsubscribe(this);
			});

		UpdateScenarioText($"""
		                    Fallen to Ruin: The monster attack modifier deck is adjusted with Ruinmaw attack modifier cards as if the first eleven perk boxes on the Ruinmaw character sheet were marked. The Ruinmaw’s Minor Empowers will be used later.

		                    The Ruinmaw cannot be reduced below 1 hit point. When the Ruinmaw is reduced to 1 hit point or would be killed, remove it from the map.

		                    Savage Stalker: The Ruinmaw can focus figures with {Icons.Inline(Icons.GetCondition(Conditions.Invisible))} and target them with abilities.

		                    Door {Icons.InlineMarker(Marker.Type._1)} is locked. At the end of the round where the Ruinmaw is removed from the map, open door {Icons.InlineMarker(Marker.Type._1)}.
		                    """);
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters roomRevealedParameters)
	{
		await base.OnRoomRevealed(roomRevealedParameters);

		if(roomRevealedParameters.OpenedDoor == _door1)
		{
			UpdateScenarioText($"""
			                    When any door {Icons.InlineMarker(Marker.Type._2)} is opened, open all doors {Icons.InlineMarker(Marker.Type._2)}.
			                    """);
		}
		else if(_doors2.Contains(roomRevealedParameters.OpenedDoor))
		{
			foreach(Door door in _doors2.Where(door => door.Opened))
			{
				await door.Open(roomRevealedParameters.PotentialOpener);
			}

			//TODO: Also check if its a ruinmaw amd (requires AMDs)
			ScenarioEvents.AMDCardDrawnEvent.Subscribe(this,
				parameters => parameters.AMDCard.Model is RuinmawEmpowerAMDCard && parameters.AbilityState.Performer is Monster monster &&
				              monster.MonsterModel is Hound or GiantViper,
				async parameters =>
				{
					parameters.SetOverrideAMDCardModel(ModelDB.AMDCard<PlusZeroAMDCard>());
					if(parameters.AMDCard.Model is RuinmawEmpowerAMDCard)
					{
						_remainingEmpowerCount--;
						AMDCard card = new AMDCard(ModelDB.AMDCard<RuinmawEmpowerAMDCard>(), parameters.AbilityState.Performer.AMDCardDeck.Owner);
						card.DrawnEvent += OnEmpowerDrawn;
						parameters.AbilityState.Performer.AMDCardDeck.DiscardPile.Add(card);
					}

					await GDTask.CompletedTask;
				});

			ScenarioEvents.FigureKilledEvent.Subscribe(this,
				parameters => (parameters.PotentialKiller is Monster monster && monster.MonsterModel is RuinmawBoss &&
				               monster.EnemiesWith(parameters.Figure)) ||
				              (parameters.PotentialKillSource is Wound or Rupture && parameters.Figure.Alignment != "Enemies"),
				async _ =>
				{
					Figure performer = GameController.Instance.Map.Figures.FirstOrDefault(figure =>
						figure is Monster monster && monster.MonsterModel is RuinmawBoss);
					if(performer != null)
					{
						await HealEmpower(performer);
					}
				});

			ScenarioEvents.LosingCardToNegateDamageEvent.Subscribe(this,
				parameters => parameters.SufferDamageParameters.PotentialDamageDealer is Monster monster && monster.MonsterModel is RuinmawBoss &&
				              monster.EnemiesWith(parameters.Character) && parameters.ResultingCardState == CardState.Lost,
				async parameters =>
				{
					await HealEmpower(parameters.SufferDamageParameters.PotentialDamageDealer);
				}, order: 100);

			ScenarioEvents.RoundEndedEvent.Subscribe(this,
				_ => !GameController.Instance.Map.Figures.Any(figure => figure is Monster monster && monster.MonsterModel is RuinmawBossRoom3),
				async _ =>
				{
					await _door3.Open(null);
					ScenarioEvents.RoundEndedEvent.Unsubscribe(this);
				});

			UpdateScenarioText($"""
			                    The Ruinmaw cannot be reduced below 1 hit point. When the Ruinmaw is reduced to 1 hit point or would be killed, remove it from the map.

			                    Savage Stalker: The Ruinmaw can focus figures with {Icons.Inline(Icons.GetCondition(Conditions.Invisible))} and target them with abilities.

			                    Hunt the Hunter: Hounds and Giant Vipers are allies to each other and enemies to all other monster types.
			                    If a Hound or Giant Viper draws a Ruinmaw attack modifier or Empower card as part of an attack, treat the revealed card as {Icons.Inline(Icons.GetAMDValue("+0"))} with no other eﬀect and discard it instead.

			                    Belly of the Beast: Each time the Ruinmaw kills an enemy, an enemy loses an ability card to negate {Icons.Inline(Icons.Damage)} caused by the Ruinmaw, or an enemy dies to {Icons.Inline(Icons.Damage)} from {Icons.Inline(Icons.GetCondition(Conditions.Wound1))} or {Icons.Inline(Icons.GetCondition(Conditions.Rupture))}, the Ruinmaw performs {Icons.Inline(Icons.Heal)}{_healValue}, {Icons.Inline(Icons.GetCondition(Ruinmaw.Empower))}, {Icons.Inline(Icons.GetCondition(Ruinmaw.Empower))}, self.

			                    Door {Icons.InlineMarker(Marker.Type._3)} is locked. At the end of the round where the Ruinmaw is removed from the map, open door {Icons.InlineMarker(Marker.Type._3)}.
			                    """);
		}
		else if(roomRevealedParameters.OpenedDoor == _door3)
		{
			List<Character> recoveredCard = [];
			ScenarioEvents.FigureTurnEndedEvent.Subscribe(this,
				parameters => parameters.Figure is Character character && !recoveredCard.Contains(character) &&
				              character.Hex.GetRoom() == GameController.Instance.Map.Rooms[3],
				async parameters =>
				{
					Character character = (Character)parameters.Figure;
					recoveredCard.Add(character);

					AbilityCard selectedAbilityCard =
						await AbilityCmd.SelectAbilityCard(character, CardState.Lost, hintText: "Select a lost card to recover");

					await AbilityCmd.ReturnToHand(selectedAbilityCard);
				});

			UpdateScenarioText(
				$"A Taste of Power: The first time each character ends their turn on A4A, that character may {Icons.Inline(Icons.RecoverCard)} one lost card. ");
		}
		else if(roomRevealedParameters.OpenedDoor == _door4)
		{
			Monster ruinmaw =
				(Monster)GameController.Instance.Map.Figures.First(figure => figure is Monster monster && monster.MonsterModel is RuinmawBossRoom5);

			ScenarioCheckEvents.ShieldCheckEvent.Subscribe(this,
				canApplyParameters => canApplyParameters.Figure == ruinmaw,
				applyParameters =>
				{
					applyParameters.AdjustShield(RemainingShamans());
				});

			ScenarioEvents.SufferDamageEvent.Subscribe(this,
				canApplyParameters => canApplyParameters.Figure == ruinmaw && canApplyParameters.FromAttack,
				async applyParameters =>
				{
					applyParameters.AdjustShield(RemainingShamans());
					await GDTask.CompletedTask;
				});

			ScenarioCheckEvents.RetaliateCheckEvent.Subscribe(this,
				canApplyParameters => canApplyParameters.Figure == ruinmaw,
				applyParameters =>
				{
					applyParameters.AddRetaliate(RemainingShamans(), RemainingShamans());
				});

			ScenarioEvents.RetaliateEvent.Subscribe(this,
				parameters => parameters.RetaliatingFigure == ruinmaw &&
				              RangeHelper.Distance(parameters.AbilityState.Performer.Hex, parameters.RetaliatingFigure.Hex) <=
				              RemainingShamans(),
				async applyParameters =>
				{
					applyParameters.AdjustRetaliate(RemainingShamans());
					await GDTask.CompletedTask;
				});

			ScenarioEvents.FigureKilledEvent.Subscribe(this, new object(),
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel is VermlingShaman && !ruinmaw.IsDead,
				async _ =>
				{
					ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();
					ScenarioCheckEvents.RetaliateCheckEvent.FireChangedEvent();
					await GDTask.CompletedTask;
				}, EffectType.Visuals);


			SatedIndicator satedIndicator = ResourceLoader.Load<PackedScene>("res://Content/Classes/Ruinmaw/SatedIndicator.tscn")
				.Instantiate<SatedIndicator>();
			ruinmaw.AddChild(satedIndicator);
			satedIndicator.Modulate = Colors.White;
			satedIndicator.SelfModulate = Colors.White;
			((RuinmawBossRoom5)ruinmaw.MonsterModel).SatedIndicator = satedIndicator;
			satedIndicator.Hide();

			ScenarioEvents.FigureKilledEvent.Subscribe(this, new object(),
				canApplyParameters => canApplyParameters.Figure is Monster monster && monster.MonsterModel is VermlingShaman && !ruinmaw.IsDead,
				async _ =>
				{
					//TODO: Teleport to hex of dead Shaman
					await HealEmpower(ruinmaw);
					((RuinmawBossRoom5)ruinmaw.MonsterModel).Sate(ruinmaw);
				});

			//TODO: Change sated word to sated icon (requires AMDs)
			UpdateScenarioText($"""
			                    The Ruinmaw can now be reduced below 1 hit point and killed.

			                    Savage Stalker: The Ruinmaw can focus figures with {Icons.Inline(Icons.GetCondition(Conditions.Invisible))} and target them with abilities.

			                    Belly of the Beast: Each time the Ruinmaw kills an enemy, an enemy loses an ability card to negate {Icons.Inline(Icons.Damage)} caused by the Ruinmaw, or an enemy dies to {Icons.Inline(Icons.Damage)} from {Icons.Inline(Icons.GetCondition(Conditions.Wound1))} or {Icons.Inline(Icons.GetCondition(Conditions.Rupture))}, the Ruinmaw performs {Icons.Inline(Icons.Heal)}{_healValue}, {Icons.Inline(Icons.GetCondition(Ruinmaw.Empower))}, {Icons.Inline(Icons.GetCondition(Ruinmaw.Empower))}, self.

			                    Ruined Ritual: The Ruinmaw gains {Icons.Inline(Icons.Shield)}X  and  {Icons.Inline(Icons.Retaliate)}X, {Icons.Inline(Icons.Range)}X, where X is the number of Vermling Shamans alive.

			                    Devour Whole: Immediately after each time a Vermling Shaman dies, the Ruinmaw jumps into the hex where that enemy died, gains the benefits of Belly of the Beast, and the Ruinmaw becomes Sated. While Sated, the Ruinmaw gains a bonus to special abilities they perform denoted with a Sated icon. The Sated status lasts until the end of the Ruinmaw’s next turn.
			                    """);
		}
	}

	private static int RemainingShamans()
	{
		return GameController.Instance.Map.Figures.Count(figure => figure is Monster monster && monster.MonsterModel is VermlingShaman);
	}

	private async GDTask HealEmpower(Figure performer)
	{
		await new ActionState(performer, [
				HealAbility.Builder()
					.WithHealValue(_healValue)
					.WithAfterHealPerformedSubscription(
						ScenarioEvents.AfterHealPerformed.Subscription.New(
							applyFunction: async parameters =>
							{
								await Empower(parameters.AbilityState, parameters.AbilityState.Target);
								await Empower(parameters.AbilityState, parameters.AbilityState.Target);
							}))
					.WithTarget(Target.Self)
			])
			.Perform();
	}

	public async GDTask Empower(AbilityState potentialAbilityState, Figure figure)
	{
		Figure potentialConditionGiver = potentialAbilityState?.Authority;

		ScenarioEvents.InflictConditions.Parameters inflictConditionsParameters =
			await ScenarioEvents.InflictConditionsEvent.CreatePrompt(
				new ScenarioEvents.InflictConditions.Parameters(potentialAbilityState, figure, [Ruinmaw.Empower]), figure);

		foreach(ConditionModel conditionModel in inflictConditionsParameters.ConditionModels)
		{
			ScenarioEvents.InflictCondition.Parameters inflictConditionParameters =
				await ScenarioEvents.InflictConditionEvent.CreatePrompt(
					new ScenarioEvents.InflictCondition.Parameters(potentialAbilityState, figure, potentialConditionGiver, conditionModel), figure);

			if(!inflictConditionParameters.Prevented)
			{
				if(conditionModel == Ruinmaw.Empower)
				{
					if(_remainingEmpowerCount == 0)
					{
						continue;
					}

					_remainingEmpowerCount--;
					AMDCard card = new AMDCard(ModelDB.AMDCard<RuinmawEmpowerAMDCard>(), figure.AMDCardDeck.Owner);
					ScenarioEvents.EmpowerAdded.Parameters empowerAddedParameters =
						await ScenarioEvents.EmpowerAddedEvent.CreatePrompt(
							new ScenarioEvents.EmpowerAdded.Parameters(figure));

					card.DrawnEvent += OnEmpowerDrawn;

					figure.AMDCardDeck.AddCard(card, empowerAddedParameters.ShuffleDrawPile);

					await ScenarioEvents.ConditionAddedEvent.CreatePrompt(
						new ScenarioEvents.ConditionAdded.Parameters(potentialAbilityState, figure, potentialConditionGiver, conditionModel), figure);
				}
				else
				{
					ScenarioEvents.InflictConditionDuplicatesCheck.Parameters inflictConditionDuplicatesCheckParameters =
						await ScenarioEvents.InflictConditionDuplicatesCheckEvent.CreatePrompt(
							new ScenarioEvents.InflictConditionDuplicatesCheck.Parameters(potentialAbilityState, figure, conditionModel), figure);

					if(!inflictConditionDuplicatesCheckParameters.Prevented)
					{
						if(inflictConditionDuplicatesCheckParameters.AddStack)
						{
							await figure.AddConditionStack(conditionModel);
						}
						else
						{
							await figure.AddCondition(conditionModel, potentialAbilityState?.Performer);
						}

						await ScenarioEvents.ConditionAddedEvent.CreatePrompt(
							new ScenarioEvents.ConditionAdded.Parameters(potentialAbilityState, figure, potentialConditionGiver, conditionModel),
							figure);
					}
				}
			}
		}
	}

	private void OnEmpowerDrawn(AMDCard card)
	{
		_remainingEmpowerCount++;
	}
}
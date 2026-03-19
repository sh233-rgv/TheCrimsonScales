using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ScenarioRM003 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioRM003.tscn";
	public override string ScenarioPrefix => "RM";
	public override int ScenarioNumber => 3;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<RMScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new CustomScenarioGoals(
		$"Have all characters occupy hexes {Icons.InlineMarker(Marker.Type.a)} or exhaust on a hex {Icons.InlineMarker(Marker.Type.a)} to win this scenario.");

	private string _text;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();
		
		GameController.Instance.Map.Treasures[0].SetItemLoot(ModelDB.Item<SerratedEdge>());

		IEnumerable<Hex> markerAHexes = GameController.Instance.Map.GetMarkers(Marker.Type.a).Select(marker => marker.Hex);
		
		List<Objective> objectives = GameController.Instance.Map.GetChildrenOfType<Objective>();
		foreach(Objective objective in objectives)
		{
			objective.Init(1, "Crate");
		}

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			_ => GameController.Instance.Map.Figures.Where(figure => figure is Character)
				.All(character => markerAHexes.Contains(character.Hex)),
			async _ =>
			{
				await ((CustomScenarioGoals)ScenarioGoals).Win();
			});

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => parameters.Figure is Character && !markerAHexes.Contains(parameters.Figure.Hex),
			async _ =>
			{
				await ((CustomScenarioGoals)ScenarioGoals).Lose();
			}
		);

		ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(this,
			parameters => parameters.Performer.Alignment == "Enemies" && parameters.AbilityState.Target is Character character &&
			              character.Conditions.Any(condition => condition.ConditionModel.IsNegative) /*TODO: && Scenario Effects Check,*/,
			async parameters =>
			{
				parameters.AbilityState.SingleTargetSetHasAdvantage();
				await GDTask.CompletedTask;
			});

		ScenarioCheckEvents.MoveCheckEvent.Subscribe(this,
			canApplyParameters => canApplyParameters.Performer is Monster monster && monster.MonsterModel is Ooze &&
			                      canApplyParameters.Hex.HasHexObjectOfType<Water>(),
			applyParameters =>
			{
				applyParameters.SetMoveCost(1);
			}
		);

		ScenarioCheckEvents.FigureInfoItemExtraEffectsCheckEvent.Subscribe(this,
			parameters => parameters.Figure is Monster monster && monster.MonsterModel is Ooze,
			parameters =>
			{
				parameters.Add(new InfoTextExtraEffect.Parameters("This figure treats water tiles as corridors."));
			}
		);

		ScenarioEvents.AbilityStartedEvent.Subscribe(this,
			parameters => parameters.AbilityState is SummonAbility.State && parameters.Performer is Monster monster && monster.MonsterModel is Ooze,
			async parameters =>
			{
				((MonsterSummonAbility.State)parameters.AbilityState).SetGetValidHexes((state, figures) =>
				{
					figures.AddRange(RangeHelper.GetHexesInRange(state.Performer.Hex, 1)
						.Where(hex => hex.IsEmpty() || (hex.IsUnoccupied() && hex.HasHexObjectOfType<Water>())));
				});
				await GDTask.CompletedTask;
			});

		_text = $"""
		         If any character becomes exhausted while not occupying a hex {Icons.InlineMarker(Marker.Type.a)}, the scenario is lost.

		         Hungry Predators: All enemies gain advantage on all attacks targeting characters with one or more negative conditions, as a scenario effect.
		         """;
		UpdateScenarioText("River of Decay: Scavenging Oozes have been attracted to the Vermling corpses floating down the river and treat hexes with water as corridors for the purposes of movement and summon abilities.");
	}

	protected override async GDTask OnRoomRevealed(ScenarioEvents.RoomRevealed.Parameters roomRevealedParameters)
	{
		await base.OnRoomRevealed(roomRevealedParameters);

		if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[1])
		{
			Hex markerBHex = GameController.Instance.Map.GetMarker(Marker.Type.a).Hex;
			ScenarioEvents.ActionStartedEvent.Subscribe(this,
				actionStartedParameters =>
					actionStartedParameters.ActionState.Performer is Character character && character.Hex == markerBHex &&
					((actionStartedParameters.ActionState.ActionSource is AbilityCardSide cardSide && cardSide.Model.Loss) ||
					 (actionStartedParameters.ActionState.ActionSource is ItemModel itemModel && itemModel.ItemUseType is ItemUseType.Consume)),
				async actionStartedParameters =>
				{
					ScenarioEvents.AbilityStartedEvent.Subscribe(this, actionStartedParameters.ActionState,
						parameters => parameters.AbilityState.ActionState == actionStartedParameters.ActionState &&
						              parameters.AbilityState is HealAbility.State or AttackAbility.State or MoveAbility.State,
						async parameters =>
						{
							switch(parameters.AbilityState)
							{
								case HealAbility.State healAbilityState:
									healAbilityState.AbilityAdjustHealValue(2);
									break;
								case AttackAbility.State attackAbilityState:
									attackAbilityState.AbilityAdjustAttackValue(2);
									break;
								case MoveAbility.State moveAbilityState:
									moveAbilityState.AdjustMoveValue(2);
									break;
							}

							await GDTask.CompletedTask;
						});
					ScenarioEvents.ActionEndedEvent.Subscribe(this, actionStartedParameters.ActionState,
						parameters => parameters.ActionState == actionStartedParameters.ActionState,
						async _ =>
						{
							ScenarioEvents.AbilityStartedEvent.Unsubscribe(this, actionStartedParameters.ActionState);
							ScenarioEvents.ActionEndedEvent.Unsubscribe(this, actionStartedParameters.ActionState);
							await GDTask.CompletedTask;
						});
					await GDTask.CompletedTask;
				});

			_text += $"""

			          Vermling Ritual Site: It’s hard to tell what magics the rodents were working, but some of its power lingers.
			          When any character performs an action with {Icons.Inline(Icons.LoseCard)} while occupying hex {Icons.InlineMarker(Marker.Type.b)}, that character adds +2{Icons.Inline(Icons.Attack)} to all their attacks, +2{Icons.Inline(Icons.Move)} to all their moves, and +2{Icons.Inline(Icons.Heal)} to all their heals for that action.
			          """;
			UpdateScenarioText("");
		}
		else if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[3])
		{
			ScenarioEvents.FigureKilledEvent.Subscribe(this, new object(),
				parameters => parameters.Figure is Objective,
				async parameters =>
				{
					await AbilityCmd.SpawnCoin(parameters.Figure.Hex, parameters.Figure);
					await AbilityCmd.SpawnCoin(parameters.Figure.Hex, parameters.Figure);
				});

			_text += """

			         Crates are objectives with 1 hit point. When destroyed, they drop 2 money tokens.
			         """;
			UpdateScenarioText("");
		}
	}

	protected override void UpdateScenarioText(string text)
	{
		if(string.IsNullOrEmpty(text))
		{
			text = "\n\n" + text;
		}

		base.UpdateScenarioText(_text + text);
	}
}
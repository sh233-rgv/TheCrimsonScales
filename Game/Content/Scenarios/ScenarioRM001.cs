using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ScenarioRM001 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/ScenarioRM001.tscn";
	public override string ScenarioPrefix => "RM";
	public override int ScenarioNumber => 1;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<RMScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillAllEnemiesScenarioGoals();

	private IEnumerable<Door> _doors1;
	private IEnumerable<Door> _doors2;

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		_doors1 = GameController.Instance.Map.GetMarkers(Marker.Type._1).Select(marker => marker.GetHexObject<Door>());
		_doors2 = GameController.Instance.Map.GetMarkers(Marker.Type._2).Select(marker => marker.GetHexObject<Door>());

		GameController.Instance.Map.Treasures[0].SetItemLoot(ModelDB.Item<GrislyBoots>());

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => parameters.PotentialKiller is Character && parameters.PotentialKiller.EnemiesWith(parameters.Figure),
			async parameters =>
			{
				//TODO: Scenario Effects Check
				await AbilityCmd.AddCondition(null, parameters.PotentialKiller, Conditions.Muddle);
			});

		ScenarioEvents.InflictConditionEvent.Subscribe(this,
			parameters =>
				parameters.Target.Alignment is "Enemies" &&
				AbilityCmd.CheckImmunity(parameters.ConditionModel, Conditions.Immobilize),
			async parameters =>
			{
				parameters.SetPrevented(true);

				await GDTask.CompletedTask;
			}
		);

		ScenarioCheckEvents.ImmunitiesVisualCheckEvent.Subscribe(this,
			parameters => parameters.Figure.Alignment is "Enemies",
			parameters =>
			{
				parameters.AddImmunity(Conditions.Immobilize);
			}
		);

		ScenarioEvents.DuringAttackEvent.Subscribe(this,
			parameters => parameters.Performer is Monster monster && monster.MonsterModel is BanditArcher or BanditGuard,
			async parameters =>
			{
				if(parameters.Performer is Monster monster && monster.MonsterModel is BanditArcher)
				{
					parameters.AbilityState.SingleTargetAdjustPull(1);
				}
				else
				{
					parameters.AbilityState.SingleTargetAdjustPush(1);
				}

				await GDTask.CompletedTask;
			});

		UpdateScenarioText($"""
		                    Pervasive Mind: Whenever a character kills an enemy, that character gains {Icons.Inline(Icons.GetCondition(Conditions.Muddle))} as a scenario effect.

		                    Parasitic Influence I: All enemies are immune to {Icons.Inline(Icons.GetCondition(Conditions.Immobilize))}.

		                    Hired Muscle: All Bandit Guards add {Icons.Inline(Icons.Push)}1 to all their attacks, and all Bandit Archers add {Icons.Inline(Icons.Pull)}1 to all their attacks.

		                    Whenever a door {Icons.InlineMarker(Marker.Type._1)} is opened, open all doors {Icons.InlineMarker(Marker.Type._1)}
		                    """);
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

			ScenarioEvents.InflictConditionEvent.Subscribe(this, new object(),
				parameters =>
					parameters.Target.Alignment is "Enemies" &&
					AbilityCmd.CheckImmunity(parameters.ConditionModel, Conditions.Disarm),
				async parameters =>
				{
					parameters.SetPrevented(true);

					await GDTask.CompletedTask;
				}
			);

			ScenarioCheckEvents.ImmunitiesVisualCheckEvent.Subscribe(this, new object(),
				parameters => parameters.Figure.Alignment is "Enemies",
				parameters =>
				{
					parameters.AddImmunity(Conditions.Disarm);
				}
			);

			ScenarioEvents.FigureKilledEvent.Subscribe(this, new object(),
				parameters => parameters.Figure is Monster monster && monster.MonsterModel is InoxArcher or InoxGuard &&
				              monster.MonsterType is MonsterType.Elite,
				async parameters =>
				{
					await SpawnMonster(null, ((Monster)parameters.Figure).MonsterModel, MonsterType.Normal, parameters.Figure.Hex);
				});

			ScenarioCheckEvents.SpawnCoinCheckEvent.Subscribe(this,
				parameters => parameters.Dropper is Monster monster && monster.MonsterModel is InoxArcher or InoxGuard &&
				              monster.MonsterType is MonsterType.Elite,
				parameters =>
				{
					parameters.SetCoinsToSpawn(0);
				});

			UpdateScenarioText($"""
			                    Pervasive Mind: Whenever a character kills an enemy, that character gains {Icons.Inline(Icons.GetCondition(Conditions.Muddle))} as a scenario effect.

			                    Parasitic Influence II: All enemies are immune to {Icons.Inline(Icons.GetCondition(Conditions.Immobilize))} and {Icons.Inline(Icons.GetCondition(Conditions.Disarm))}.

			                    Enslaved Laborers: Whenever an elite Inox Guard or Inox Arhcer is killed, that enemy does not drop a money token. Instead, spawn one normal enemy of that same type in the hex that the elite died in.

			                    Whenever a door {Icons.InlineMarker(Marker.Type._2)} is opened, open all doors {Icons.InlineMarker(Marker.Type._2)}
			                    """);
		}
		else if(roomRevealedParameters.Room == GameController.Instance.Map.Rooms[2])
		{
			foreach(Door door in _doors2.Where(door => !door.Opened))
			{
				await door.Open(roomRevealedParameters.PotentialOpener);
			}

			ScenarioEvents.InflictConditionEvent.Subscribe(this, new object(),
				parameters =>
					parameters.Target.Alignment is "Enemies" &&
					AbilityCmd.CheckImmunity(parameters.ConditionModel, Conditions.Stun),
				async parameters =>
				{
					parameters.SetPrevented(true);

					await GDTask.CompletedTask;
				}
			);

			ScenarioCheckEvents.ImmunitiesVisualCheckEvent.Subscribe(this, new object(),
				parameters => parameters.Figure.Alignment is "Enemies",
				parameters =>
				{
					parameters.AddImmunity(Conditions.Stun);
				}
			);

			UpdateScenarioText($"""
			                    Pervasive Mind: Whenever a character kills an enemy, that character gains {Icons.Inline(Icons.GetCondition(Conditions.Muddle))} as a scenario effect.

			                    Parasitic Influence III: All enemies are immune to {Icons.Inline(Icons.GetCondition(Conditions.Immobilize))}, {Icons.Inline(Icons.GetCondition(Conditions.Disarm))}, and {Icons.Inline(Icons.GetCondition(Conditions.Stun))}.

			                    Hired Muscle: All Bandit Guards add {Icons.Inline(Icons.Push)}1 to all their attacks, and all Bandit Archers add {Icons.Inline(Icons.Pull)}1 to all their attacks.

			                    Enslaved Laborers: Whenever an elite Inox Guard or Inox Arhcer is killed, that enemy does not drop a money token. Instead, spawn one normal enemy of that same type in the hex that the elite died in.
			                    """);
		}
	}
}
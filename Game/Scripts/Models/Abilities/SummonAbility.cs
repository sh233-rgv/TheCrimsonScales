using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;
using GTweens.Easings;

public class SummonAbility : ActiveAbility<SummonAbility.State>
{
	public class State : ActiveAbilityState
	{
		public Summon Summon { get; private set; }

		public void SetSummon(Summon summon)
		{
			Summon = summon;
		}
	}

	private readonly SummonStats _summonStats;
	private readonly string _name;
	private readonly string _texturePath;
	private readonly Action<State, List<Hex>> _getValidHexes;

	public SummonAbility(SummonStats summonStats, string name, string texturePath, Action<State, List<Hex>> getValidHexes = null,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getHintText = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, getHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		_summonStats = summonStats;
		_name = name;
		_texturePath = texturePath;
		_getValidHexes = getValidHexes;
	}

	protected override async GDTask Perform(State abilityState)
	{
		// Target a hex within range
		Hex targetedHex = await AbilityCmd.SelectHex(abilityState, list =>
		{
			if(_getValidHexes == null)
			{
				RangeHelper.FindHexesInRange(abilityState.Performer.Hex, 1, true, list);

				for(int i = list.Count - 1; i >= 0; i--)
				{
					Hex hex = list[i];

					if(!hex.IsEmpty())
					{
						list.RemoveAt(i);
					}
				}
			}
			else
			{
				_getValidHexes(abilityState, list);
			}
		});

		if(targetedHex != null)
		{
			PackedScene summonScene = ResourceLoader.Load<PackedScene>("res://Scenes/Scenario/Summon.tscn");
			Summon summon = summonScene.Instantiate<Summon>();
			GameController.Instance.Map.AddChild(summon);
			await summon.Init(targetedHex);
			summon.Spawn(_summonStats, (Character)abilityState.Performer, _name, _texturePath);
			abilityState.SetSummon(summon);

			summon.Scale = Vector2.Zero;
			await summon.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardableAsync();


			ScenarioEvents.FigureKilledEvent.Subscribe(abilityState, this,
				canApplyParameters => canApplyParameters.Figure == summon,
				async applyParameters =>
				{
					await abilityState.ActionState.RequestDiscardOrLose();
				});

			await Activate(abilityState);
		}
	}

	protected override async GDTask Activate(State abilityState)
	{
		await base.Activate(abilityState);
	}

	protected override async GDTask Deactivate(State abilityState)
	{
		await base.Deactivate(abilityState);

		ScenarioEvents.FigureKilledEvent.Unsubscribe(abilityState, this);

		if(abilityState.Summon != null && !abilityState.Summon.IsDead)
		{
			await abilityState.Summon.Destroy();
		}
	}
}
using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ProjectileAbility : ActiveAbility<ProjectileAbility.State>
{
	public class State : ActiveAbilityState
	{
		public List<BombardProjectileToken> Tokens { get; } = new List<BombardProjectileToken>();

		public void AddToken(BombardProjectileToken token)
		{
			Tokens.Add(token);
		}
	}

	private readonly Func<Hex, List<Ability>> _getAbilities;

	public int Range { get; }
	public int Targets { get; }
	public AbilityCardSide AbilityCardSide { get; }

	public ProjectileAbility(int range, Func<Hex, List<Ability>> getAbilities, AbilityCardSide abilityCardSide, int targets = 1,
		Func<State, GDTask> onAbilityStarted = null, Func<State, GDTask> onAbilityEnded = null, Func<State, GDTask> onAbilityEndedPerformed = null,
		ConditionalAbilityCheckDelegate conditionalAbilityCheck = null,
		Func<State, string> getHintText = null,
		List<ScenarioEvents.AbilityStarted.Subscription> abilityStartedSubscriptions = null,
		List<ScenarioEvents.AbilityEnded.Subscription> abilityEndedSubscriptions = null,
		List<ScenarioEvent<ScenarioEvents.AbilityPerformed.Parameters>.Subscription> abilityPerformedSubscriptions = null)
		: base(onAbilityStarted, onAbilityEnded, onAbilityEndedPerformed, conditionalAbilityCheck, getHintText, abilityStartedSubscriptions, abilityEndedSubscriptions, abilityPerformedSubscriptions)
	{
		_getAbilities = getAbilities;
		Range = range;
		AbilityCardSide = abilityCardSide;
		Targets = targets;
	}

	protected override async GDTask Perform(State abilityState)
	{
		for(int i = 0; i < Targets; i++)
		{
			Hex targetedHex = await AbilityCmd.SelectHex(abilityState, list =>
			{
				RangeHelper.FindHexesInRange(abilityState.Performer.Hex, Range, true, list);
			}, hintText: "Select a hex to target with the Projectile ability");

			if(targetedHex != null)
			{
				BombardProjectileToken token = ResourceLoader.Load<PackedScene>("res://Content/Classes/Bombard/BombardProjectile.tscn").Instantiate<BombardProjectileToken>();
				GameController.Instance.Map.AddChild(token);
				token.SetCardSide(AbilityCardSide);

				await token.Init(targetedHex);

				abilityState.AddToken(token);
			}
		}

		if(abilityState.Tokens.Count > 0)
		{
			await Activate(abilityState);

			ScenarioEvents.FigureTurnStartedEvent.Subscribe(abilityState, this,
				canApplyParameters => canApplyParameters.Figure == abilityState.Performer,
				async applyParameters =>
				{
					foreach(BombardProjectileToken token in abilityState.Tokens)
					{
						bool targetFound = false;

						foreach(Figure figure in token.Hex.GetHexObjectsOfType<Figure>())
						{
							if(abilityState.Authority.EnemiesWith(figure))
							{
								targetFound = true;
							}
						}

						if(targetFound)
						{
							// Perform the actual abilities
							ActionState actionState = new ActionState(abilityState.Performer, _getAbilities(token.Hex), abilityState.ActionState);
							await actionState.Perform();
						}
					}

					await abilityState.ActionState.RequestDiscardOrLose();
				}
			);
		}
	}

	protected override async GDTask Activate(State abilityState)
	{
		await base.Activate(abilityState);
	}

	protected override async GDTask Deactivate(State abilityState)
	{
		await base.Deactivate(abilityState);

		ScenarioEvents.FigureTurnStartedEvent.Unsubscribe(abilityState, this);

		foreach(BombardProjectileToken token in abilityState.Tokens)
		{
			await token.Destroy();
		}
	}
}
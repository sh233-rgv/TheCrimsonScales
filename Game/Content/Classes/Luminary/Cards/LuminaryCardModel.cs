using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public abstract class LuminaryCardModel<TTop, TBottom> : AtlasAbilityCardModel<TTop, TBottom>
	where TTop : LuminaryCardSide, new()
	where TBottom : LuminaryCardSide, new()
{
	protected override string TexturePath => "res://Content/Classes/Luminary/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class LuminaryCardSide : AbilityCardSide
{
	public class GlowAbilityModel(List<Element> elements, Func<List<Element>, Ability> ability, string hintText, string hintIcon)
	{
		public List<Element> Elements { get; } = elements;
		public Func<List<Element>, Ability> Ability { get; } = ability;
		public string HintText { get; } = hintText;
		public string HintIcon { get; } = hintIcon;
	}

	protected AbilityCardAbility Scuttle(int distance, IReadOnlyCollection<Element> possibleElements)
	{
		return new AbilityCardAbility(MoveAbility.Builder()
			.WithDistance(distance)
			.WithMoveType(MoveType.Jump)
			.WithConditionalAbilityCheck(async state =>
			{
				await GDTask.CompletedTask;

				return state.ActionState.GetAbilityState<AttackAbility.State>(0).Performed;
			})
			.WithOnAbilityEndedPerformed(async state =>
			{
				if(possibleElements.Count == 1)
				{
					await AbilityCmd.InfuseElement(state, possibleElements.First());
				}
				else
				{
					await AbilityCmd.InfuseElement(state, possibleElements);
				}
			})
			.WithOnAbilityStarted(async state =>
			{
				ScenarioCheckEvents.MoveCanStopAtCheckEvent.Subscribe(state.Performer, this,
					parameters =>
						parameters.AbilityState == state && !state.ActionState.GetAbilityState<AttackAbility.State>(0).GetEmptyAOEHexes()
							.Contains(parameters.Hex),
					parameters =>
					{
						parameters.SetCannotStopAt();
					}
				);

				await GDTask.CompletedTask;
			})
			.WithOnAbilityEnded(async state =>
				{
					ScenarioCheckEvents.MoveCanStopAtCheckEvent.Unsubscribe(state.Performer, this);

					await GDTask.CompletedTask;
				}
			)
			.Build());
	}

	protected AbilityCardAbility PerformFreeGlow()
	{
		return new AbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				GlowActiveAbility.State abilityState = (GlowActiveAbility.State)((Character)state.Performer).Cards
					.SelectMany(card => card.ActiveActionStates.SelectMany(actionState => actionState.AbilityStates))
					.FirstOrDefault(abilityState => abilityState is GlowActiveAbility.State);
				if(abilityState != null)
				{
					await GlowAbility(state.Performer, abilityState.GlowAbilityModels, false);
					state.SetPerformed();
				}

				await GDTask.CompletedTask;
			})
			.Build());
	}

	private static async GDTask GlowAbility(Figure performer, GlowAbilityModel[] glowAbilities, bool consumeElements = true)
	{
		List<ScenarioEvents.GenericChoice.Subscription> subscriptions = [];
		foreach(GlowAbilityModel glowAbility in glowAbilities)
		{
			if(consumeElements)
			{
				subscriptions.Add(ScenarioEvents.GenericChoice.Subscription.ConsumeElements(glowAbility.Elements,
					applyFunction: async parameters =>
					{
						ActionState actionState = new ActionState(performer, [glowAbility.Ability(glowAbility.Elements)]);
						await actionState.Perform();
					},
					effectInfoViewParameters: new TextEffectInfoView.Parameters(glowAbility.HintText)
				));
			}
			else
			{
				subscriptions.Add(ScenarioEvents.GenericChoice.Subscription.New(
					applyFunction: async applyParameters =>
					{
						ActionState actionState = new ActionState(performer, [glowAbility.Ability([])]);
						await actionState.Perform();
					},
					effectButtonParameters: new IconEffectButton.Parameters(glowAbility.HintIcon),
					effectInfoViewParameters: new TextEffectInfoView.Parameters(glowAbility.HintText),
					effectType: EffectType.SelectableMandatory
				));
			}
		}

		await AbilityCmd.GenericChoice(performer, subscriptions, hintText: "Select a glow ability to perform");
	}

	protected class GlowActiveAbility : ActiveAbility<GlowActiveAbility.State>
	{
		public class State : ActiveAbilityState
		{
			public GlowAbilityModel[] GlowAbilityModels { get; set; }
		}

		public GlowAbilityModel[] GlowAbilities;

		public new class AbstractBuilder<TBuilder, TAbility> : ActiveAbility<State>.AbstractBuilder<TBuilder, TAbility>,
			AbstractBuilder<TBuilder, TAbility>.IGlowAbilityStep
			where TBuilder : AbstractBuilder<TBuilder, TAbility>
			where TAbility : GlowActiveAbility, new()
		{
			public interface IGlowAbilityStep
			{
				TBuilder WithGlowAbility(params GlowAbilityModel[] glowAbilities);
			}

			public TBuilder WithGlowAbility(params GlowAbilityModel[] glowAbilities)
			{
				Obj.GlowAbilities = glowAbilities;
				return (TBuilder)this;
			}
		}

		public class GlowBuilder : AbstractBuilder<GlowBuilder, GlowActiveAbility>
		{
			internal GlowBuilder() { }
		}

		public static GlowBuilder Builder()
		{
			return new GlowBuilder();
		}


		protected override async GDTask Perform(State abilityState)
		{
			await AskConfirmAndActivate(abilityState);
		}

		protected override async GDTask Activate(State abilityState)
		{
			await base.Activate(abilityState);
			ActionState actionState = ((Character)abilityState.Performer).Cards
				.SelectMany(card => card.ActiveActionStates)
				.FirstOrDefault(actionState => actionState.AbilityStates.Any(state => state is State));

			if(actionState != null)
			{
				await actionState.RequestDiscardOrLose();
			}

			AbilityCmd.SubscribeDuringCharacterTurn(ScenarioEvents.GetSubscriberPair(abilityState, this), EffectType.Selectable,
				character => character == abilityState.Performer &&
				             GlowAbilities.Any(glowAbility =>
					             glowAbility.Elements.All(e =>
						             GameController.Instance.ElementManager.GetState(e) > ElementState.Inert)),
				async character =>
				{
					await GlowAbility(character, GlowAbilities);
					await GDTask.CompletedTask;
				},
				effectButtonParameters: new IconEffectButton.Parameters("res://Content/Classes/Luminary/Glow.svg"),
				effectInfoViewParameters: new TextEffectInfoView.Parameters(
					$"Perform {Icons.Inline("res://Content/Classes/Luminary/Glow.svg")}"));

			abilityState.GlowAbilityModels = GlowAbilities;
			await GDTask.CompletedTask;
		}

		protected override async GDTask Deactivate(State abilityState)
		{
			await base.Deactivate(abilityState);

			AbilityCmd.UnsubscribeDuringTurn(ScenarioEvents.GetSubscriberPair(abilityState, this));
		}
	}
}
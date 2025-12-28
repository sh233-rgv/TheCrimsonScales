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
		public List<Element> Elements = elements;
		public Func<List<Element>, Ability> Ability = ability;
		public string HintText = hintText;
		public string HintIcon = hintIcon;
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
					await AbilityCmd.InfuseElement(possibleElements.First(), state.Authority, state);
				}
				else
				{
					await AbilityCmd.InfuseElement(possibleElements, state.Authority, state);
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

	protected AbilityCardAbility PerformGlow()
	{
		return new AbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				foreach(AbilityCard abilityCard in ((Character)state.Performer).Cards)
				{
					AbilityState glowState =
						abilityCard.ActiveActionStates
							.SelectMany(a => a.AbilityStates)
							.FirstOrDefault(s => s.GetCustomValue<bool>(state.Performer, "Active Glow"));
					if(glowState != null)
					{
						await GlowAbility(state.Performer, glowState.GetCustomValue<GlowAbilityModel[]>(state.Performer, "Glow Perform"), false);
						break;
					}
				}

				await GDTask.CompletedTask;
			})
			.Build());
	}

	protected AbilityCardAbility Glow(params GlowAbilityModel[] glowAbilities)
	{
		return new AbilityCardAbility(OtherActiveAbility.Builder()
			.WithOnActivate(async state =>
			{
				foreach(AbilityCard abilityCard in ((Character)state.Performer).Cards)
				{
					foreach(ActionState actionState in abilityCard.ActiveActionStates)
					{
						if(actionState.AbilityStates.Any(abilityState => abilityState.GetCustomValue<bool>(state.Performer, "Active Glow")))
						{
							await actionState.RequestDiscardOrLose();
							break;
						}
					}
				}

				AbilityCmd.SubscribeDuringCharacterTurn(ScenarioEvents.GetSubscriberPair(state, this), EffectType.Selectable,
					character => character == state.Performer &&
					             glowAbilities.Any(glowAbility =>
						             glowAbility.Elements.All(e =>
							             GameController.Instance.ElementManager.GetState(e) > ElementState.Inert)),
					async character =>
					{
						await GlowAbility(character, glowAbilities);
						await GDTask.CompletedTask;
					},
					effectButtonParameters: new IconEffectButton.Parameters("res://Content/Classes/Luminary/Glow.svg"),
					effectInfoViewParameters: new TextEffectInfoView.Parameters(
						$"Perform {Icons.Inline("res://Content/Classes/Luminary/Glow.svg")}"));

				state.SetCustomValue(state.Performer, "Active Glow", true);
				state.SetCustomValue(state.Performer, "Glow Perform", glowAbilities);
				await GDTask.CompletedTask;
			})
			.WithOnDeactivate(async state =>
			{
				ScenarioEvents.FigureTurnStartedEvent.Unsubscribe(state, this);
				ScenarioEvents.AbilityEndedEvent.Unsubscribe(state, this);

				await GDTask.CompletedTask;
			})
			.Build());
	}

	private async GDTask GlowAbility(Figure performer, GlowAbilityModel[] glowAbilities, bool consumeElements = true)
	{
		List<ScenarioEvents.GenericChoice.Subscription> subscriptions = [];
		foreach(GlowAbilityModel glowAbility in glowAbilities)
		{
			if(consumeElements)
			{
				subscriptions.Add(ScenarioEvents.GenericChoice.Subscription.ConsumeElements(glowAbility.Elements,
					applyFunction: async parameters =>
					{
						ActionState actionState = new(performer, [glowAbility.Ability(glowAbility.Elements)]);
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
}
using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public abstract class ShardrenderCardModel<TTop, TBottom> : AbilityCardModel<TTop, TBottom>
	where TTop : ShardrenderCardSide
	where TBottom : ShardrenderCardSide
{
	protected override string TexturePath => "res://Content/Classes/Shardrender/Cards.jpg";
	protected override int ColumnCount => 8;
	protected override int RowCount => 4;
}

public abstract class ShardrenderCardSide : AbilityCardSideModel
{
	public const string CrystallizeIconPath = "res://Content/Classes/Shardrender/Crystallize.svg";
	public const string CrystallizeForwardIconPath = "res://Content/Classes/Shardrender/CrystallizeForward.svg";

	protected OtherAbility.OtherBuilder MoveCharacterTokenBackAbility(DynamicInt<OtherAbility.State> count, bool canBeDifferent = true)
	{
		return OtherAbility.Builder()
			.WithPerformAbility(async state =>
			{
				Dictionary<AbilityCard, CrystallizeAbility.State> possibilities = GetActiveCrystallizeStates(state.Performer as Character);

				int actualCount = count.GetValue(state);

				if(possibilities.Count == 0 || actualCount == 0)
				{
					return;
				}

				if(possibilities.Count == 1)
				{
					await MoveCharacterTokenBack(possibilities.Values.First(), actualCount);
					state.SetPerformed();

					return;
				}

				if(canBeDifferent)
				{
					CrystallizeAbility.State higherCount = possibilities.Values.MaxBy(crystallizeState => crystallizeState.UseSlotIndex);
					CrystallizeAbility.State lowerCount = possibilities.Values.MinBy(crystallizeState => crystallizeState.UseSlotIndex);
					while(actualCount > lowerCount.UseSlotIndex)
					{
						await higherCount.MoveBackUseSlot();
						higherCount = possibilities.Values.MaxBy(crystallizeState => crystallizeState.UseSlotIndex);
						lowerCount = possibilities.Values.MinBy(crystallizeState => crystallizeState.UseSlotIndex);
						actualCount--;
						state.SetPerformed();
					}

					for(int i = 0; i < actualCount; i++)
					{
						AbilityCard abilityCard = await AbilityCmd.SelectAbilityCard(state.Performer,
							cards => cards.AddRange(possibilities.Keys), null,
							hintText: $"Select a {Icons.HintText(CrystallizeIconPath)} to move the character token backward one slot.");
						if(abilityCard != null)
						{
							await possibilities[abilityCard].MoveBackUseSlot();
							state.SetPerformed();
						}
					}
				}
				else
				{
					AbilityCard abilityCard = await AbilityCmd.SelectAbilityCard(state.Performer,
						cards => cards.AddRange(possibilities.Keys), null,
						hintText:
						$"Select a {Icons.HintText(CrystallizeIconPath)} to move the character token backward {actualCount} slot{(actualCount > 1 ? "s" : "")}.");
					if(abilityCard != null)
					{
						await MoveCharacterTokenBack(possibilities[abilityCard], count.GetValue(state));
						state.SetPerformed();
					}
				}
			});
	}

	private async GDTask MoveCharacterTokenBack(CrystallizeAbility.State state, int count)
	{
		for(int i = 0; i < count; i++)
		{
			if(state.UseSlotIndex > 0)
			{
				await state.MoveBackUseSlot();
			}
		}
	}

	protected ScenarioEvent<T>.Subscription AdvanceCrystallizeSubscription<T>(Func<T, GDTask> applyFunction,
		EffectInfoViewParameters effectInfoViewParameters, bool canApplyMultipleTimesDuringSubscription = false)
		where T : ScenarioEvent.ParametersBaseWithAbilityState
	{
		return ScenarioEvent<T>.Subscription.New(
			parameters => GetActiveCrystallizeStates(parameters.BaseAbilityState.Performer as Character).Any(),
			async parameters =>
			{
				Dictionary<AbilityCard, CrystallizeAbility.State> possibilities =
					GetActiveCrystallizeStates(parameters.BaseAbilityState.Performer as Character);
				if(possibilities.Count == 1)
				{
					await possibilities.First().Value.AdvanceUseSlot();
				}
				else
				{
					AbilityCard abilityCard = await AbilityCmd.SelectAbilityCard(parameters.BaseAbilityState.Performer,
						cards => cards.AddRange(possibilities.Keys), null, true,
						hintText: $"Select a {Icons.HintText(CrystallizeIconPath)} to move the character token backward one slot.");

					await possibilities[abilityCard].AdvanceUseSlot();
				}

				await applyFunction(parameters);
			}, EffectType.Selectable, canApplyMultipleTimesDuringSubscription: canApplyMultipleTimesDuringSubscription,
			effectButtonParameters: new IconEffectButton.Parameters(CrystallizeForwardIconPath),
			effectInfoViewParameters: effectInfoViewParameters);
	}

	protected Dictionary<AbilityCard, CrystallizeAbility.State> GetActiveCrystallizeStates(Character character)
	{
		Dictionary<AbilityCard, CrystallizeAbility.State> possibilities = [];
		foreach(AbilityCard card in character.Cards)
		{
			if(card.CardState is CardState.Persistent)
			{
				foreach(ActionState activeActionState in card.ActiveActionStates)
				{
					CrystallizeAbility.State crystallizeState = (CrystallizeAbility.State)
						activeActionState.AbilityStates.FirstOrDefault(abilityState => abilityState is CrystallizeAbility.State);
					if(crystallizeState != null && crystallizeState.UseSlotIndex > 0)
					{
						possibilities[card] = crystallizeState;
					}
				}
			}
		}

		return possibilities;
	}

	protected async GDTask<bool> AdvanceCrystallizeConditionalAbilityCheck(Figure figure, EffectInfoViewParameters effectInfoViewParameters)
	{
		Dictionary<AbilityCard, CrystallizeAbility.State> possibilities =
			GetActiveCrystallizeStates(figure as Character);
		if(possibilities.Count == 0)
		{
			return false;
		}

		bool movedCrystallizeForward = false;
		await AbilityCmd.GenericChoice(figure,
			[
				ScenarioEvents.GenericChoice.Subscription.New(
					_ => true,
					async _ =>
					{
						if(possibilities.Count == 1)
						{
							await possibilities.First().Value.AdvanceUseSlot();
						}
						else
						{
							AbilityCard abilityCard = await AbilityCmd.SelectAbilityCard(figure,
								cards => cards.AddRange(possibilities.Keys), null, true,
								hintText: $"Select a {Icons.HintText(CrystallizeIconPath)} to move the character token backward one slot.");

							await possibilities[abilityCard].AdvanceUseSlot();
						}

						movedCrystallizeForward = true;
						await GDTask.CompletedTask;
					}, EffectType.Selectable,
					effectButtonParameters: new IconEffectButton.Parameters(CrystallizeForwardIconPath),
					effectInfoViewParameters: effectInfoViewParameters)
			], hintText: $"{Icons.HintText(CrystallizeForwardIconPath)} to perform ability?");
		return movedCrystallizeForward;
	}
}
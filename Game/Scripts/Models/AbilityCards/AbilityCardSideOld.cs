// using System;
// using System.Collections.Generic;
// using Fractural.Tasks;
// using Godot;
// using GTweens.Easings;
//
// public abstract class AbilityCardSideOld : IAbilitySource
// {
// 	private Character _performer;
//
// 	public AbilityCard AbilityCard { get; init; }
// 	public AbilityCardActionState ActionState { get; private set; }
//
// 	public async GDTask Perform(Character performer)
// 	{
// 		// Performer = performer;
//
// 		_performer = performer;
// 		ActionState = new AbilityCardActionState(this);
//
// 		AbilityParameters parameters = new AbilityParameters(ActionState, _performer, _performer);
//
// 		await Perform(parameters);
//
// 		if(AbilityCard.CardState == CardState.Playing)
// 		{
// 			await Discard();
// 		}
// 	}
//
// 	public async GDTask RemoveFromActive()
// 	{
// 		await ActionState.RemoveFromActive();
// 	}
//
// 	protected async GDTask<SummonAbilityState> Summon(AbilityParameters parameters,
// 		SummonStats summonStats, string name, string texturePath, Action<List<Hex>> getValidHexes = null)
// 	{
// 		Hex targetedHex = null;
// 		Summon summon = null;
//
// 		SummonAbilityState state = await AbilityCmd.StartAbility(new SummonAbilityState(parameters,
// 			async state =>
// 			{
// 				PackedScene summonScene = ResourceLoader.Load<PackedScene>("res://Scenes/Scenario/Summon.tscn");
// 				summon = summonScene.Instantiate<Summon>();
// 				GameController.Instance.Map.AddChild(summon);
// 				summon.Init(targetedHex);
// 				summon.Spawn(summonStats, (Character)parameters.Performer, name, texturePath);
//
// 				summon.Scale = Vector2.Zero;
// 				await summon.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardableAsync();
//
// 				ScenarioEvents.FigureKilledEvent.Subscribe(this, canApplyParameters =>
// 					{
// 						return canApplyParameters.Figure == summon;
// 					},
// 					async applyParameters =>
// 					{
// 						await AbilityCmd.DiscardOrLose(AbilityCard);
// 					});
// 			},
// 			async state =>
// 			{
// 				ScenarioEvents.FigureKilledEvent.Unsubscribe(this);
//
// 				if(summon != null && !summon.IsDestroyed)
// 				{
// 					await summon.Destroy();
// 				}
// 			}));
//
// 		if(state.IsBlocked)
// 		{
// 			return await AbilityCmd.EndAbility(state);
// 		}
//
// 		// Target a hex within range
// 		targetedHex = await AbilityCmd.SelectHex(state, list =>
// 		{
// 			if(getValidHexes == null)
// 			{
// 				RangeHelper.FindHexesInRange(state.Performer.Hex, 1, true, list);
//
// 				for(int i = list.Count - 1; i >= 0; i--)
// 				{
// 					Hex hex = list[i];
//
// 					if(!hex.IsEmpty())
// 					{
// 						list.RemoveAt(i);
// 					}
// 				}
// 			}
// 			else
// 			{
// 				getValidHexes(list);
// 			}
// 		});
//
// 		// Activate the ability
// 		if(targetedHex != null)
// 		{
// 			await state.Activate();
// 			await parameters.ActionState.ActiveAbility(state);
// 			state.SetPerformed();
// 		}
//
// 		return await AbilityCmd.EndAbility(state);
// 	}
//
// 	protected async GDTask Infuse(Element element)
// 	{
// 		if(HasPerformedAction())
// 		{
// 			await AbilityCmd.InfuseElement(element);
// 		}
// 	}
//
// 	protected GDTask<bool> Consume(Element element)
// 	{
// 		return AbilityCmd.TryConsumeElement(element);
// 	}
//
// 	protected GDTask GainXP()
// 	{
// 		return GainXP(1);
// 	}
//
// 	protected async GDTask GainXP(int xp)
// 	{
// 		if(HasPerformedAction())
// 		{
// 			await AbilityCmd.GainXP(_performer, xp);
// 		}
// 	}
//
// 	protected async GDTask ForceGainXP(int xp)
// 	{
// 		await AbilityCmd.GainXP(_performer, xp);
// 	}
//
// 	private async GDTask Discard()
// 	{
// 		await AbilityCmd.DiscardCard(AbilityCard);
// 	}
//
// 	protected async GDTask Loss()
// 	{
// 		if(HasPerformedAction())
// 		{
// 			await AbilityCmd.LoseCard(AbilityCard);
// 		}
// 	}
//
// 	protected async GDTask Round()
// 	{
// 		if(HasPerformedAction())
// 		{
// 			if(AbilityCard.CardState == CardState.PlayingActive)
// 			{
// 				await AbilityCmd.RoundCard(this);
// 			}
// 		}
// 	}
//
// 	protected async GDTask RoundLoss()
// 	{
// 		if(HasPerformedAction())
// 		{
// 			if(AbilityCard.CardState == CardState.PlayingActive)
// 			{
// 				await AbilityCmd.RoundLossCard(this);
// 			}
// 			else
// 			{
// 				await AbilityCmd.LoseCard(AbilityCard);
// 			}
// 		}
// 	}
//
// 	protected async GDTask Persistent()
// 	{
// 		if(HasPerformedAction())
// 		{
// 			if(AbilityCard.CardState == CardState.PlayingActive)
// 			{
// 				await AbilityCmd.PersistentCard(this);
// 			}
// 		}
// 	}
//
// 	protected async GDTask PersistentLoss()
// 	{
// 		if(HasPerformedAction())
// 		{
// 			if(AbilityCard.CardState == CardState.PlayingActive)
// 			{
// 				await AbilityCmd.PersistentLossCard(this);
// 			}
// 			else
// 			{
// 				await AbilityCmd.LoseCard(AbilityCard);
// 			}
// 		}
// 	}
//
// 	protected GDTask UseSlots(AbilityParameters parameters, List<UseSlot> useSlots,
// 		Func<UseSlotAbilityState, GDTask> onActivate, Func<UseSlotAbilityState, GDTask> onDeactivate)
// 	{
// 		return AbilityCmd.UseSlots(parameters, useSlots, onActivate, onDeactivate, () => AbilityCmd.DiscardOrLose(AbilityCard));
// 	}
//
// 	private bool HasPerformedAction()
// 	{
// 		return ActionState.HasPerformed;
// 	}
//
// 	protected abstract GDTask Perform(AbilityParameters parameters);
// }


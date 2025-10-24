using Fractural.Tasks;
using System.Collections.Generic;
using Godot;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices;

public class Chill : ConditionModel
{
	public override string Name => "Chill";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Chill.svg";
	public override bool CanBeUpgraded => true;
	public override bool RemovedAtEndOfTurn => true;
	public override ConditionModel[] ImmunityCompareBaseCondition => [Conditions.Immobilize, Conditions.Muddle];

	public override async GDTask Add(Figure target, ConditionNode node)
	{
		await base.Add(target, node);
		_appliedDuringThisTurn = false;
		int totalChill = NumChill();
		if(totalChill > 1)
		{
			Node.Destroy();
			Node = null;
			Chill secondChill = (Chill)Owner.Conditions.Where(c => c.ImmutableInstance ==
				Conditions.Chill).Skip(1).FirstOrDefault();
			ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Unsubscribe(secondChill);
			SetChillText();
			ScenarioEvents.InflictConditionDuplicatesCheckEvent.Unsubscribe(this);
		}
        else
        {
			SubscribeToChill();
        }
	}

	public override GDTask Remove()
	{
		if(NumChill() > 1)
		{
			Node?.Destroy();
			Node = null;
			Owner.Conditions.Remove(this);
			SetChillText();
			ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Unsubscribe(this);
			Chill firstChill = (Chill)Owner.Conditions.Where(c => c.ImmutableInstance == Conditions.Chill).FirstOrDefault();
			ScenarioEvents.FigureTurnEndingEvent.Subscribe(firstChill, cansubscribe => true, subscription =>
				{
					ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Subscribe(firstChill, TurnEndedCanApply,
						TurnEndedApply, EffectType.MandatoryBeforeOptionals);
					ScenarioEvents.FigureTurnEndingEvent.Unsubscribe(firstChill);
					return GDTask.CompletedTask;
				},
				EffectType.MandatoryAfterOptionals);
			
			return GDTask.CompletedTask;
		}
		else
		{
			return base.Remove();
		}
	}
	
	private void SetChillText()
    {
		Chill lastChill = (Chill)Owner.Conditions.LastOrDefault(c => c.ImmutableInstance == Conditions.Chill);
		lastChill.Node.SetStackText(NumChill() == 1 ? null : NumChill().ToString());
    }
	
	protected override GDTask DuplicatesCheckApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		parameters.SetPrevented(true);

		return GDTask.CompletedTask;
	}

	protected override bool DuplicatesCheckCanApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		if(parameters.Condition is Chill)
		{
			return false;
		}
		return base.DuplicatesCheckCanApply(parameters);
	}

	private void SubscribeToChill()
	{
		ScenarioEvents.AbilityStartedEvent.Subscribe(
			this,
			parameters => parameters.Performer == Owner &&
						 (parameters.AbilityState is AttackAbility.State || parameters.AbilityState is MoveAbility.State),
			parameters =>
			{
				int currentStacks = NumChill();

				if(parameters.AbilityState is AttackAbility.State attackState)
				{
					attackState.AbilityAdjustAttackValue(-currentStacks);
				}
				else if(parameters.AbilityState is MoveAbility.State moveState)
				{
					moveState.AdjustMoveValue(-currentStacks);
				}
				Node.Flash();
				return GDTask.CompletedTask;
			},
			EffectType.MandatoryBeforeOptionals
		);
	}
	
	private int NumChill()
	{
		return Owner.Conditions.Count(c => c.ImmutableInstance == Conditions.Chill);
    }
}

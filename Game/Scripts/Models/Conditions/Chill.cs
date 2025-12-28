using Fractural.Tasks;
using Godot;

public class Chill : ConditionModel
{
	public override string Name => "Chill";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Chill.svg";
	public override ConditionPolarity ConditionPolarity => ConditionPolarity.Negative;
	public override bool CanBeAppliedMultipleTimesOnSingleTarget => true;
	public override ConditionModel[] ImmunityCompareBaseConditions => [Conditions.Immobilize, Conditions.Muddle];
	public override bool RemovedAtEndOfTurn => true;
	public override bool Stackable => true;

	public override async GDTask OnAdded(Condition condition)
	{
		await base.OnAdded(condition);

		_appliedDuringThisTurn = false;
		int totalChill = GetChillCount();
		if(totalChill > 1)
		{
			Chill secondChill = (Chill)Owner.Conditions
				.Where(c => c.ImmutableInstance == Conditions.Chill).Skip(1).FirstOrDefault();
			ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Unsubscribe(secondChill);
			SetChillText();
			ScenarioEvents.InflictConditionDuplicatesCheckEvent.Unsubscribe(this);
		}
		else
		{
			SubscribeToChill();
		}
	}

	public override GDTask OnRemoved(Condition condition)
	{
		if(GetChillCount() > 1)
		{
			Owner.Conditions.Remove(this);
			SetChillText();
			ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Unsubscribe(this);
			Chill firstChill = (Chill)Owner.Conditions.FirstOrDefault(c => c.ImmutableInstance == Conditions.Chill);
			ScenarioEvents.FigureTurnEndingEvent.Subscribe(firstChill, cansubscribe => true, parameters =>
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
			return base.OnRemoved(condition);
		}
	}

	private void SetChillText()
	{
		Chill lastChill = (Chill)Owner.Conditions.LastOrDefault(c => c.ImmutableInstance == Conditions.Chill);
		lastChill.Node.SetStackText(GetChillCount() == 1 ? null : GetChillCount().ToString());
	}

	protected override GDTask DuplicatesCheckApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		parameters.SetPrevented(true);

		return GDTask.CompletedTask;
	}

	protected override bool DuplicatesCheckCanApply(ScenarioEvents.InflictConditionDuplicatesCheck.Parameters parameters)
	{
		if(parameters.ConditionModel is Chill)
		{
			return false;
		}

		return base.DuplicatesCheckCanApply(parameters);
	}

	private void SubscribeToChill()
	{
		ScenarioEvents.AbilityStartedEvent.Subscribe(
			this,
			parameters =>
				parameters.Performer == Owner &&
				(parameters.AbilityState is AttackAbility.State || parameters.AbilityState is MoveAbility.State),
			parameters =>
			{
				int currentStacks = GetChillCount();

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

	private int GetChillCount()
	{
		return Owner.Conditions.Count(c => c.ImmutableInstance == Conditions.Chill);
	}
}
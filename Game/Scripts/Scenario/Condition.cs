using Fractural.Tasks;
using Godot;

public class Condition : IEventSubscriber
{
	private readonly EventSubscriberPair _subscriberPair;

	private bool _appliedDuringThisTurn;

	public ConditionModel ConditionModel { get; private set; }
	public ConditionHexObjectEffectView EffectView { get; private set; }
	public Figure Owner { get; private set; }
	public Figure PotentialCauser { get; private set; }

	public int StackCount { get; private set; }

	public Condition(ConditionModel conditionModel, Figure owner, Figure potentialCauser)
	{
		ConditionModel = conditionModel;
		Owner = owner;
		PotentialCauser = potentialCauser;
		StackCount = 1;

		if(conditionModel.RequiresCauser && potentialCauser == null)
		{
			Log.Error($"Trying to add {conditionModel.Name} to {owner.DisplayName}, but {nameof(potentialCauser)} is null.");
		}

		// Create a custom subscriber pair, so it can't interfere with subscriptions added by Condition Models
		_subscriberPair = ScenarioEvents.GetSubscriberPair(this, new object());
	}

	public async GDTask OnAdded()
	{
		if(ConditionModel.ShouldShowOnFigure)
		{
			EffectView = Owner.AddEffectView<ConditionHexObjectEffectView>(new ConditionHexObjectEffectView.Parameters(ConditionModel));
		}

		if(Owner.TakingTurn)
		{
			_appliedDuringThisTurn = true;
		}

		ScenarioEvents.InflictConditionDuplicatesCheckEvent.Subscribe(_subscriberPair,
			parameters =>
			{
				if(parameters.Prevented || parameters.Target != Owner)
				{
					return false;
				}

				// Block either the exact same condition, or the same condition of a lower level, like regular Poison for Poison 2
				return
					parameters.ConditionModel.BaseLevelCondition == ConditionModel.BaseLevelCondition &&
					parameters.ConditionModel.UpgradableLevel <= ConditionModel.UpgradableLevel;
			},
			async parameters =>
			{
				parameters.SetPrevented(true);

				if(parameters.Target.TakingTurn)
				{
					_appliedDuringThisTurn = true;
				}

				await GDTask.CompletedTask;
			}
		);

		if(ConditionModel.RemovedAtEndOfTurn)
		{
			ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Subscribe(_subscriberPair,
				parameters => parameters.Figure == Owner,
				async parameters =>
				{
					if(_appliedDuringThisTurn)
					{
						_appliedDuringThisTurn = false;
					}
					else
					{
						await AbilityCmd.RemoveCondition(this);
					}
				}
			);
		}

		// Remove any condition that is a lower level of this condition
		for(int i = Owner.Conditions.Count - 1; i >= 0; i--)
		{
			Condition condition = Owner.Conditions[i];
			if(condition.ConditionModel.BaseLevelCondition == ConditionModel.BaseLevelCondition && condition != this)
			{
				await AbilityCmd.RemoveCondition(condition);
			}
		}

		await ConditionModel.OnAdded(this);

		// Conditions like Bless and Curse are removed immediately after being added to a figure
		if(ConditionModel.ImmediatelyRemovedOnApply)
		{
			await AbilityCmd.RemoveCondition(this);
		}
	}

	public async GDTask OnRemoved()
	{
		if(EffectView != null)
		{
			Owner.RemoveEffectView(EffectView);
		}

		await ConditionModel.OnRemoved(this);

		ScenarioEvents.InflictConditionDuplicatesCheckEvent.Unsubscribe(_subscriberPair);
		ScenarioEvents.FigureTurnEndedConditionsFallOffEvent.Unsubscribe(_subscriberPair);
	}

	public void Flash()
	{
		EffectView?.Flash();
	}
}
using Fractural.Tasks;
using Godot;

public class Condition : IEventSubscriber
{
	private bool _appliedDuringThisTurn;

	public ConditionModel ConditionModel { get; private set; }
	public ConditionHexObjectEffectView ConditionHexObjectEffectView { get; private set; }
	public Figure Owner { get; private set; }
	public Figure PotentialCauser { get; private set; }

	public int StackCount { get; private set; }

	public Condition(ConditionModel conditionModel, ConditionHexObjectEffectView conditionHexObjectEffectView, Figure owner, Figure potentialCauser)
	{
		ConditionModel = conditionModel;
		ConditionHexObjectEffectView = conditionHexObjectEffectView;
		Owner = owner;
		PotentialCauser = potentialCauser;
		StackCount = 1;

		if(conditionModel.RequiresCauser && potentialCauser == null)
		{
			Log.Error($"Trying to add {conditionModel.Name} to {owner.DisplayName}, but {nameof(potentialCauser)} is null.");
		}
	}

	public async GDTask OnAdded()
	{
		if(Owner.TakingTurn)
		{
			_appliedDuringThisTurn = true;
		}

		await ConditionModel.OnAdded(this);

		if(ConditionModel.ImmediatelyRemovedOnApply)
		{
			await AbilityCmd.RemoveCondition(this);
		}
	}

	public async GDTask OnRemoved()
	{
		await ConditionModel.OnRemoved(this);
	}

	public void Flash()
	{
		ConditionHexObjectEffectView.Flash();
	}
}
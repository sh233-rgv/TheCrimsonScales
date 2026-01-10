public abstract class ConditionEnhancement<T> : EnhancementModel<TargetedAbilityState>
	where T : ConditionModel
{
	protected override string TexturePath => Icons.GetCondition(ModelDB.Condition<T>());

	protected override void _Enhance(TargetedAbilityState state)
	{
		state.AbilityAddCondition(ModelDB.Condition<T>());
	}
}
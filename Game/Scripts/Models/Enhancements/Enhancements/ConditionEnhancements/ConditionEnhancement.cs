public abstract class ConditionEnhancement<T> : EnhancementModel<IConditionsAbilityState>
	where T : ConditionModel
{
	protected override string TexturePath => Icons.GetCondition(ModelDB.Condition<T>());

	protected override void _Enhance(IConditionsAbilityState state, EnhancementMark enhancementMark)
	{
		state.AbilityAddCondition(ModelDB.Condition<T>());
	}
}
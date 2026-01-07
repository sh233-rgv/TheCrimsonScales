public abstract class ConditionEnhancement<T> : EnhancementModel
	where T : ConditionModel
{
	protected override string TexturePath => Icons.GetCondition(ModelDB.Condition<T>());
}
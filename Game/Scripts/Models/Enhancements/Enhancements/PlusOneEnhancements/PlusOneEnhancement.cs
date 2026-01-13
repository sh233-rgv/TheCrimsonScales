public abstract class PlusOneEnhancement<TState> : EnhancementModel<TState>
	where TState : AbilityState
{
	protected override string TexturePath => Icons.PlusOneEnhancement;

	public override bool DefaultDoubleCostOnDoubleTarget => true;
}
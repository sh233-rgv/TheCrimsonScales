public class PlusOneRangeEnhancement : PlusOneEnhancement<TargetedAbilityState>
{
	public override int BaseCost => 30;

	protected override void Enhance(TargetedAbilityState state)
	{
		state.AbilityAdjustRange(1);
	}
}
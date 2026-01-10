public class PlusOnePullEnhancement : PlusOneEnhancement<TargetedAbilityState>
{
	public override int BaseCost => 20;

	protected override void _Enhance(TargetedAbilityState state)
	{
		state.AbilityAdjustPull(1);
	}
}
public class PlusOnePushEnhancement : PlusOneEnhancement<TargetedAbilityState>
{
	public override int BaseCost => 30;

	protected override void _Enhance(TargetedAbilityState state)
	{
		state.AbilityAdjustPush(1);
	}
}
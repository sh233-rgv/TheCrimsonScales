public class PlusOneTargetEnhancement : PlusOneEnhancement<TargetedAbilityState>
{
	public override int BaseCost => 75;

	protected override void Enhance(TargetedAbilityState state)
	{
		state.AdjustTargets(1);
	}
}
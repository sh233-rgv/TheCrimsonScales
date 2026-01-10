public class PlusOneShieldEnhancement : PlusOneEnhancement<ShieldAbility.State>
{
	public override int BaseCost => 80;

	protected override void _Enhance(ShieldAbility.State state, EnhancementMark enhancementMark)
	{
		state.AdjustAdditionalShield(1);
	}
}
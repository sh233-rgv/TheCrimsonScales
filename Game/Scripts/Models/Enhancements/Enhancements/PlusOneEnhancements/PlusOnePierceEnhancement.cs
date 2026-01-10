public class PlusOnePierceEnhancement : PlusOneEnhancement<AttackAbility.State>
{
	public override int BaseCost => 30;

	protected override void _Enhance(AttackAbility.State state)
	{
		state.AbilityAdjustPierce(1);
	}
}
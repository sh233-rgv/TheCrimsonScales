public class PlusOneAttackEnhancement : PlusOneEnhancement<AttackAbility.State>
{
	public override int BaseCost => 50;

	protected override void _Enhance(AttackAbility.State state)
	{
		state.AbilityAdjustAttackValue(1);
	}
}
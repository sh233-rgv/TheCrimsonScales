public class PlusOneRetaliateEnhancement : PlusOneEnhancement<RetaliateAbility.State>
{
	public override int BaseCost => 60;

	protected override void _Enhance(RetaliateAbility.State state, EnhancementMark enhancementMark)
	{
		state.AdjustRetaliateValue(1);
	}
}
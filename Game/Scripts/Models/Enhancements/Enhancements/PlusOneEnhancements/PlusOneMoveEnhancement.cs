public class PlusOneMoveEnhancement : PlusOneEnhancement<MoveAbility.State>
{
	public override int BaseCost => 30;

	protected override void _Enhance(MoveAbility.State state)
	{
		state.AdjustMoveValue(1);
	}
}
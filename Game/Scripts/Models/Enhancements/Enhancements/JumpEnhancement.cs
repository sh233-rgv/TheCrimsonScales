public class JumpEnhancement : EnhancementModel<MoveAbility.State>
{
	protected override string TexturePath => Icons.Jump;
	public override int BaseCost => 60;

	protected override void Enhance(MoveAbility.State state)
	{
		state.AddJump();
	}
}
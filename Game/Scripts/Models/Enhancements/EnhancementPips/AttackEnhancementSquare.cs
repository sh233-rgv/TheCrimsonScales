public class AttackEnhancementSquare : EnhancementPipModel
{
	public override EnhancementModel[] PossibleEnhancements { get; } = [Enhancements.PlusOneAttack];
}
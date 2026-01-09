public class AttackEnhancementSquare : AttackEnhancementPip
{
	public override EnhancementModel[] PossibleEnhancements { get; } = [Enhancements.PlusOneAttack];
}
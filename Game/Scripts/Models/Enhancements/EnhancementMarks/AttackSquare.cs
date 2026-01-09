using Godot;

public class AttackSquare : AttackEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } = [Enhancements.PlusOneAttack];

	public AttackSquare(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
		: base(abilityCardSideModel, normalizedPosition)
	{
	}
}
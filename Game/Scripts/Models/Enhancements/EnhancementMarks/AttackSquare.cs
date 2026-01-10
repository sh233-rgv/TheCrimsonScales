using Godot;

public class AttackSquare : AttackEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } = [ModelDB.Enhancement<PlusOneAttackEnhancement>()];

	public AttackSquare(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
using Godot;

public abstract class MoveEnhancementMark : EnhancementMark
{
	protected MoveEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
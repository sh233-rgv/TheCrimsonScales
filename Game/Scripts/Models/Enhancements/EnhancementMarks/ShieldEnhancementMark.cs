using Godot;

public abstract class ShieldEnhancementMark : EnhancementMark
{
	public ShieldEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
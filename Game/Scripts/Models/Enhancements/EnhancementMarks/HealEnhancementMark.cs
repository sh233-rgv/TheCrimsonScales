using Godot;

public abstract class HealEnhancementMark : EnhancementMark
{
	protected HealEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
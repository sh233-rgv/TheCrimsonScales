using Godot;

public abstract class RetaliateEnhancementMark : EnhancementMark
{
	public RetaliateEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
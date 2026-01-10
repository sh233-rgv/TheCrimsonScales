using Godot;

public abstract class ConditionEnhancementMark : EnhancementMark
{
	public ConditionEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
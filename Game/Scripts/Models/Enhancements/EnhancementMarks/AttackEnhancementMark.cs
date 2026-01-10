using Godot;

public abstract class AttackEnhancementMark : EnhancementMark
{
	protected AttackEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
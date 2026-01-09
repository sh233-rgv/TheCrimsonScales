using Godot;

public abstract class AttackEnhancementMark : EnhancementMark
{
	protected AttackEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
		: base(abilityCardSideModel, normalizedPosition)
	{
	}
}
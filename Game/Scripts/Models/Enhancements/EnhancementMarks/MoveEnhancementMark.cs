using Godot;

public abstract class MoveEnhancementMark : EnhancementMark
{
	protected MoveEnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
		: base(abilityCardSideModel, normalizedPosition)
	{
	}
}
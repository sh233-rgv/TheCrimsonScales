using Godot;

public abstract class EnhancementMark
{
	public Vector2 NormalizedPosition { get; }

	public abstract EnhancementModel[] PossibleEnhancements { get; }

	protected EnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
	{
		NormalizedPosition = normalizedPosition;

		abilityCardSideModel.RegisterEnhancementMark(this);
	}
}
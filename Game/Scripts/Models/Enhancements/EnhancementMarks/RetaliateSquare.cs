using Godot;

public class RetaliateSquare : RetaliateEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } = [ModelDB.Enhancement<PlusOneRetaliateEnhancement>()];

	public RetaliateSquare(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
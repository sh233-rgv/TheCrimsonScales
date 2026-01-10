using Godot;

public class AOEHexMark : EnhancementMark
{
	public Vector2I LocalCoords { get; }

	public override EnhancementModel[] PossibleEnhancements { get; } = [ModelDB.Enhancement<RedHexEnhancement>()];

	public AOEHexMark(Vector2I localCoords, AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
		LocalCoords = localCoords;
	}
}
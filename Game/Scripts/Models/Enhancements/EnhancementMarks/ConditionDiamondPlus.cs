using System.Linq;
using Godot;

public class ConditionDiamondPlus : ConditionEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
		Enhancements.PositiveConditions
			.Concat(Enhancements.Elements)
			.ToArray();

	public ConditionDiamondPlus(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
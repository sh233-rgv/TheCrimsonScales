using System.Linq;
using Godot;

public class ShieldDiamondPlus : ShieldEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
		Enhancements.PositiveConditions
			.Prepend(ModelDB.Enhancement<PlusOneShieldEnhancement>())
			.Concat(Enhancements.Elements)
			.ToArray();

	public ShieldDiamondPlus(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
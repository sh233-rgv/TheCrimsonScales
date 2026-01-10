using System.Linq;
using Godot;

public class ConditionDiamond : ConditionEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
		Enhancements.NegativeConditions
			.Concat(Enhancements.Elements)
			.ToArray();

	public ConditionDiamond(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
		: base(abilityCardSideModel, normalizedPosition)
	{
	}
}
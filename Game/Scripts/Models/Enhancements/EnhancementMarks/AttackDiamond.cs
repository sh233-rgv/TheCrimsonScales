using System.Linq;
using Godot;

public class AttackDiamond : AttackEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
		Enhancements.NegativeConditions
			.Prepend(ModelDB.Enhancement<PlusOneAttackEnhancement>())
			.Concat(Enhancements.Elements)
			.ToArray();

	public AttackDiamond(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
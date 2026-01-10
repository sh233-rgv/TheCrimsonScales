using System.Linq;
using Godot;

public class AttackCircle : AttackEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
		Enhancements.Elements
			.Prepend(ModelDB.Enhancement<PlusOneAttackEnhancement>())
			.ToArray();

	public AttackCircle(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
		: base(abilityCardSideModel, normalizedPosition, priceMultiplier)
	{
	}
}
using System.Linq;
using Godot;

public class AttackDiamond : AttackEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
		Enhancements.NegativeConditions
			.Prepend(Enhancements.PlusOneAttack)
			.Concat(Enhancements.Elements)
			.ToArray();

	public AttackDiamond(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
		: base(abilityCardSideModel, normalizedPosition)
	{
	}
}
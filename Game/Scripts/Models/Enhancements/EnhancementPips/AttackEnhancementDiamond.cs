using System.Linq;

public class AttackEnhancementDiamond : EnhancementPipModel
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
		Enhancements.NegativeConditions
			.Prepend(Enhancements.PlusOneAttack)
			.Concat(Enhancements.Elements)
			.ToArray();
}
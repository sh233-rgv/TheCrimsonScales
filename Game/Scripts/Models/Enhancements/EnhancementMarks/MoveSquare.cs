using Godot;

public class MoveSquare : MoveEnhancementMark
{
	public override EnhancementModel[] PossibleEnhancements { get; } =
	[
		ModelDB.Enhancement<PlusOneMoveEnhancement>(),
		ModelDB.Enhancement<JumpEnhancement>()
	];

	public MoveSquare(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
		: base(abilityCardSideModel, normalizedPosition)
	{
	}
}
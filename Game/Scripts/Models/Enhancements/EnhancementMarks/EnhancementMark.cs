using Godot;

public abstract class EnhancementMark
{
	public AbilityCardSideModel AbilityCardSideModel { get; }
	public Vector2 NormalizedPosition { get; }
	public Ability Ability { get; private set; }

	public abstract EnhancementModel[] PossibleEnhancements { get; }

	protected EnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition)
	{
		AbilityCardSideModel = abilityCardSideModel;
		NormalizedPosition = normalizedPosition;

		abilityCardSideModel.RegisterEnhancementMark(this);
	}

	public void SetAbility(Ability ability)
	{
		Ability = ability;
	}
}
using Godot;

public abstract class EnhancementMark
{
	public AbilityCardSideModel AbilityCardSideModel { get; }
	public Vector2 NormalizedPosition { get; }
	public float PriceMultiplier { get; }

	public Ability Ability { get; private set; }

	public abstract EnhancementModel[] PossibleEnhancements { get; }

	protected EnhancementMark(AbilityCardSideModel abilityCardSideModel, Vector2 normalizedPosition, float priceMultiplier = 1f)
	{
		AbilityCardSideModel = abilityCardSideModel;
		NormalizedPosition = normalizedPosition;
		PriceMultiplier = priceMultiplier;

		abilityCardSideModel.RegisterEnhancementMark(this);
	}

	public void SetAbility(Ability ability)
	{
		Ability = ability;
	}
}
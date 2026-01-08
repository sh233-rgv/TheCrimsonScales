public class AbilityCardSide : IActionSource
{
	public AbilityCard AbilityCard { get; }
	public AbilityCardSideModel Model { get; }
	public AbilityCardSideType AbilityCardSideType { get; }

	public AbilityCardSide(AbilityCard abilityCard, AbilityCardSideModel model)
	{
		AbilityCard = abilityCard;
		Model = model;
	}
}
using System.Collections.Generic;

public class MonsterAbilityCardElementInfusion
{
	public IReadOnlyCollection<Element> ConsumableElements { get; }
	public Element InfusedElement { get; }

	protected MonsterAbilityCardElementInfusion(IReadOnlyCollection<Element> consumableElements, Element infusedElement)
	{
		ConsumableElements = consumableElements;
		InfusedElement = infusedElement;
	}

	public static MonsterAbilityCardElementInfusion Infuse(Element infusedElement)
	{
		return new MonsterAbilityCardElementInfusion(null, infusedElement);
	}

	public static MonsterAbilityCardElementInfusion Consume(IReadOnlyCollection<Element> consumableElements, Element infusedElement)
	{
		return new MonsterAbilityCardElementInfusion(consumableElements, infusedElement);
	}

	public static MonsterAbilityCardElementInfusion ConsumeWild(Element infusedElement)
	{
		return Consume(Elements.All, infusedElement);
	}
}
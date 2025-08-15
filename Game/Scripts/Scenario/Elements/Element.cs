using System.Collections.Generic;

public enum Element
{
	Fire = 0,
	Ice = 1,
	Air = 2,
	Earth = 3,
	Light = 4,
	Dark = 5
}

public static class Elements
{
	public static readonly IReadOnlyList<Element> All = [Element.Fire, Element.Ice, Element.Air, Element.Earth, Element.Light, Element.Dark];
}
using Godot;

public partial class ElementsView : Control
{
	[Export]
	private ElementsViewElement[] _elements;

	public void SetElementState(Element element, ElementState elementState)
	{
		_elements[(int)element].SetState(elementState);
	}

	public void SetElementInfusing(Element element, bool infusing)
	{
		_elements[(int)element].SetInfusing(infusing);
	}
}
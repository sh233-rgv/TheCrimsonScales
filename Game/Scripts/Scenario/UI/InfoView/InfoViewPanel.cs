using System.Collections.Generic;
using Godot;

public partial class InfoViewPanel : Control
{
	[Export]
	private PackedScene _dividerScene;
	[Export]
	private Control _itemParent;
	[Export]
	private Control _preferredHeightContainer;
	[Export]
	private Control _panel;
	[Export]
	private ScrollContainer _scrollContainer;
	[Export]
	private MarginContainer _marginContainer;

	private readonly List<InfoItemBase> _items = new List<InfoItemBase>();
	private readonly List<Control> _dividers = new List<Control>();

	private float _maxHeight;

	public void Init(List<InfoItemParameters> parametersList, float maxHeight)
	{
		_maxHeight = maxHeight;

		foreach(InfoItemParameters parameters in parametersList)
		{
			if(_items.Count > 0)
			{
				Control divider = _dividerScene.Instantiate<Control>();
				_itemParent.AddChild(divider);
				_dividers.Add(divider);
			}

			PackedScene packedScene = ResourceLoader.Load<PackedScene>(parameters.ScenePath);
			InfoItemBase item = packedScene.Instantiate<InfoItemBase>();
			_itemParent.AddChild(item);
			item.Init(parameters);
			_items.Add(item);
		}

		SetModulate(Colors.Transparent);

		// Wait a frame for layout stuff to finish before properly resizing and showing this new panel
		this.DelayedCall(Resize);
	}

	public void Destroy()
	{
		// Destroy this panel the next frame, to leave time for the next panel to finish layout stuff
		this.DelayedCall(QueueFree);
	}

	public void SetCanClick(bool canClick)
	{
		_scrollContainer.SetMouseFilter(canClick ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore);
		this.DelayedCall(() =>
		{
			_marginContainer.SetMouseFilter(MouseFilterEnum.Ignore);
			_scrollContainer.SetMouseFilter(MouseFilterEnum.Ignore);
			if(_panel.Size.Y < _maxHeight)
			{
				_marginContainer.SetMouseFilter(canClick ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore);
			}
			else
			{
				_scrollContainer.SetMouseFilter(canClick ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore);
			}
		});
	}

	private void Resize()
	{
		float preferredHeight = _scrollContainer.Position.Y + _preferredHeightContainer.Size.Y;
		float maxHeight = _maxHeight;

		_panel.SetSize(new Vector2(_panel.Size.X, Mathf.Min(preferredHeight, maxHeight)));

		SetModulate(Colors.White);
	}
}
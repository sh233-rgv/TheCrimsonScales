using Godot;

public partial class DebugInfo : Control
{
	[Export]
	private Label _coordsLabel;

	public override void _Ready()
	{
		base._Ready();

		GameController.Instance.Selector.CoordsChangedEvent += OnSelectorCoordsChanged;
	}

	private void OnSelectorCoordsChanged(Vector2I coords, bool visible)
	{
		_coordsLabel.Text = $"({coords.X}, {coords.Y})";
	}
}
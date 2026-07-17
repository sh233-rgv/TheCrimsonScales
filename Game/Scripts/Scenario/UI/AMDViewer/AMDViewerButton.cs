using Godot;

public partial class AMDViewerButton : Control
{
	[Export]
	private BetterButton _amdButton;

	[Export]
	public AMDViewer AMDViewer;

	private bool _lockedOpen;

	public override void _Ready()
	{
		base._Ready();

		_amdButton.MouseEntered += OnMouseEntered;
		_amdButton.MouseExited += OnMouseExited;
		_amdButton.Pressed += OnButtonPressed;
	}

	private void OnMouseEntered()
	{
		OpenViewer();
	}

	private void OnMouseExited()
	{
		if(!_lockedOpen)
		{
			AMDViewer.Hide();
		}
	}

	private void OnButtonPressed()
	{
		_lockedOpen = !_lockedOpen;

		if(_lockedOpen)
		{
			OpenViewer();
		}
		else
		{
			AMDViewer.Hide();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(!_lockedOpen)
			return;

		if(@event is InputEventMouseButton mouseEvent &&
		   mouseEvent.ButtonIndex == MouseButton.Left &&
		   mouseEvent.Pressed)
		{
			Vector2 mousePosition = GetGlobalMousePosition();

			bool clickedButton = _amdButton.GetGlobalRect().HasPoint(mousePosition);

			if(!clickedButton)
			{
				_lockedOpen = false;
				AMDViewer.Hide();
			}
		}
	}

	private void OpenViewer()
	{
		AMDViewer.UpdateDeck();

		AMDViewer.Popup();

		Vector2 popupPosition = _amdButton.GlobalPosition + new Vector2(-520, 5);

		AMDViewer.Position = new Vector2I((int)popupPosition.X, (int)popupPosition.Y);
	}
}
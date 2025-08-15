using Godot;

public partial class MenuButton : Control
{
	[Export]
	private BetterButton _button;

	public override void _Ready()
	{
		base._Ready();

		_button.Pressed += OnPressed;
	}

	private void OnPressed()
	{
		AppController.Instance.PopupManager.RequestPopup(new MenuPopup.Request());
	}
}
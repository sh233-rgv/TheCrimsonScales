using Godot;

public partial class AMDViewerBox : Control
{
	[Export]
	private RichTextLabel _label;

	[Export]
	private PackedScene _deckCount;

	[Export]
	private PackedScene _discardCount;

	[Export]
	private Container _countContainer;

	private AMDViewerButton _amdViewerButton;

	private AMDCardModel _amdModel;

	public void SetAMD(AMDCardModel amdModel, int deckCount, int discardCount)
	{
		_amdModel = amdModel;
		_label.Text = amdModel.GetSimpleString(_label.GetRichTextParameters());

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;

		for(int i = 0; i < deckCount; i++)
		{
			Control circle = _deckCount.Instantiate<Control>();
			_countContainer.AddChild(circle);
		}

		for(int i = 0; i < discardCount; i++)
		{
			Control circle = _discardCount.Instantiate<Control>();
			_countContainer.AddChild(circle);
		}
	}

	private void OnMouseEntered()
	{
		_amdViewerButton.ExtraDetailView.Show();

		Vector2 popupPosition = _amdViewerButton.GlobalPosition + new Vector2(-520, -65);

		_amdViewerButton.ExtraDetailView.Position = new Vector2I((int)popupPosition.X, (int)popupPosition.Y);

		_amdViewerButton.ExtraDetailLabel.SetText(_amdModel.ToString(_amdViewerButton.ExtraDetailLabel.GetRichTextParameters()));
	}

	private void OnMouseExited()
	{
		_amdViewerButton.ExtraDetailView.Hide();
	}

	public void SetAMDViewerButton(AMDViewerButton amdViewerButton)
	{
		_amdViewerButton = amdViewerButton;
	}
}
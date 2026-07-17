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

	public AMDViewerButton AMDViewerButton;

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
		AMDViewerButton.ExtraDetailPanel.Popup();

		Vector2 popupPosition = AMDViewerButton.GlobalPosition + new Vector2(-520, -60);

		AMDViewerButton.ExtraDetailPanel.Position = new Vector2I((int)popupPosition.X, (int)popupPosition.Y);

		AMDViewerButton.ExtraDetailLabel.SetText(_amdModel.ToString(AMDViewerButton.ExtraDetailLabel.GetRichTextParameters()));
	}

	private void OnMouseExited()
	{
		AMDViewerButton.ExtraDetailPanel.Hide();
	}
}
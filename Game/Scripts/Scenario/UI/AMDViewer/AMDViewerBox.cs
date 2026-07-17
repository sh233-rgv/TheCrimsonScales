using Godot;

public partial class AMDViewerBox : Control
{
	[Export]
	private RichTextLabel _label;

	[Export]
	private PackedScene _count;

	[Export]
	private Container _countContainer;



	public void SetAMD(AMDCardModel amdModel, int count)
	{
		_label.Text = amdModel.GetSimpleString(_label.GetRichTextParameters());

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;

		for(int i = 0; i < count; i++)
		{
			Control circle = _count.Instantiate<Control>();
			_countContainer.AddChild(circle);
		}
	}

	private void OnMouseEntered()
	{

	}

	private void OnMouseExited()
	{

	}
}
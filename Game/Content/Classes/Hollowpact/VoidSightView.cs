using Godot;

public partial class VoidSightView : Control
{
	[Export]
	private VoidSightViewEye _eye;

	public override void _Ready()
	{
		base._Ready();

		Open();
	}

	public void Open()
	{
		_eye.Open();
	}
}
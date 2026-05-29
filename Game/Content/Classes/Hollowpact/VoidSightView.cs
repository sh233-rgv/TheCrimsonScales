using Godot;
using GTweensGodot.Extensions;

public partial class VoidSightView : Control
{
	[Export]
	private Control _overlay;
	[Export]
	private VoidSightViewEye _eye;

	public void Open()
	{
		Show();
		_overlay.Show();
		_overlay.TweenModulateAlpha(1f, 0f).Play(true);
		_eye.Open();
	}

	public void Close()
	{
		_overlay.TweenModulateAlpha(0f, 0f).OnComplete(() =>
		{
			_overlay.Hide();
			Hide();
		}).Play(true);
	}
}
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class EnvelopeBCircle : Control
{
	[Export]
	private Label _label;
	[Export]
	private Control _checkmark;

	public void Init(int number, bool checkmark)
	{
		_checkmark.SetVisible(checkmark);
		_label.SetText(number.ToString());
	}

	public void Check()
	{
		_checkmark.SetVisible(true);
		_checkmark.SetScale(Vector2.Zero);
		_checkmark.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).Play();
	}
}
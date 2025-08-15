using Godot;
using GTweens.Easings;
using GTweens.Tweens;

public partial class SufferDamageView : Node2D
{
	[Export]
	private Node2D _container;
	[Export]
	private Label _label;

	private GTween _tween;

	public override void _Ready()
	{
		base._Ready();

		_container.SetScale(Vector2.Zero);
		SetVisible(false);
	}

	public void Open(Figure figure, int damage)
	{
		SetGlobalPosition(figure.GlobalPosition);
		_label.SetText(damage.ToString());

		_tween?.Kill();
		Show();
		_tween = _container.TweenScale(1f, 0.2f).SetEasing(Easing.OutBack).PlayFastForwardable();
	}

	public void Close()
	{
		_tween?.Kill();
		_tween = _container.TweenScale(0f, 0.2f).OnComplete(Hide).SetEasing(Easing.InBack).PlayFastForwardable();
	}
}
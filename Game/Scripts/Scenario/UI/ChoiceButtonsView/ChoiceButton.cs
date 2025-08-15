using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class ChoiceButton : Control
{
	[Export]
	public BetterButton BetterButton { get; private set; }

	[Export]
	private bool _startActive;

	private bool _active;
	private GTween _scaleTween;

	public override void _Ready()
	{
		base._Ready();

		PivotOffset = Size * 0.5f;

		if(_startActive)
		{
			Scale = Vector2.One;
			Show();
		}
		else
		{
			Scale = Vector2.Zero;
			Hide();
		}
	}

	public void SetActive(bool active)
	{
		if(active == _active)
		{
			return;
		}

		_active = active;
		BetterButton.SetEnabled(active, active);

		_scaleTween?.Kill();
		if(active)
		{
			_scaleTween = GTweenSequenceBuilder.New()
				.AppendTime(0.05f)
				.AppendCallback(Show)
				.Append(this.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack))
				.Build().Play();
		}
		else
		{
			_scaleTween = this.TweenScale(0f, 0.15f).OnComplete(Hide).SetEasing(Easing.InBack).Play();
		}
	}
}
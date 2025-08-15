using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class HintTextView : Control
{
	[Export]
	private Control _container;
	[Export]
	private RichTextLabel _label;

	private GTween _scaleTween;

	public override void _Ready()
	{
		SetVisible(false);

		_container.MinimumSizeChanged += OnMinimumSizeChanged;
	}

	public void Open(string text)
	{
		SetVisible(true);
		_label.SetText(text);

		_scaleTween?.Kill();
		_container.SetScale(Vector2.Zero);
		_scaleTween = GTweenSequenceBuilder.New()
			.AppendTime(0.1f)
			.Append(_container.TweenScale(1f, 0.2f).SetEasing(Easing.OutBack))
			.Build().Play();
	}

	private void OnMinimumSizeChanged()
	{
		_container.PivotOffset = _container.Size * 0.5f;
	}

	public void Close(bool immediately = false)
	{
		_scaleTween?.Kill();
		if(immediately)
		{
			_container.Scale = Vector2.Zero;
		}
		else
		{
			_scaleTween = _container.TweenScale(0f, 0.2f).SetEasing(Easing.InBack).OnComplete(Hide).Play();
		}
	}
}
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class BetweenScenariosActionButton : Control
{
	[Export]
	public BetterButton BetterButton { get; private set; }

	[Export]
	private Control _container;
	[Export]
	private Panel _panel;
	[Export]
	private StyleBox _selectedStyleBox;

	private StyleBox _regularStyleBox;
	private bool _selected;
	private GTween _scaleTween;

	public override void _Ready()
	{
		base._Ready();

		_regularStyleBox = _panel.GetThemeStylebox("panel");
	}

	public void SetSelected(bool selected)
	{
		if(_selected == selected)
		{
			return;
		}

		_selected = selected;

		_panel.AddThemeStyleboxOverride("panel", selected ? _selectedStyleBox : _regularStyleBox);

		_scaleTween?.Kill();
		if(_selected)
		{
			_scaleTween = GTweenSequenceBuilder.New()
				.AppendTime(0.05f)
				.Append(_container.TweenScale(1.2f, 0.15f).SetEasing(Easing.OutBack))
				.Build().Play();
		}
		else
		{
			_scaleTween = _container.TweenScale(1f, 0.15f).SetEasing(Easing.InBack).Play();
		}
	}
}
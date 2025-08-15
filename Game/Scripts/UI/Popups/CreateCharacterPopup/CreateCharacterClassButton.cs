using System;
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class CreateCharacterClassButton : Control
{
	[Export]
	public BetterButton BetterButton { get; private set; }

	[Export]
	private Control _container;
	[Export]
	private TextureRect _textureRect;
	[Export]
	private Control _colorOutline;
	[Export]
	private Control _inactiveOverlay;
	[Export]
	private TextureRect _iconTexture;
	[Export]
	private TextureRect _iconShadowTexture;

	private bool _selected;
	private GTween _scaleTween;

	public ClassModel ClassModel { get; private set; }

	public event Action<CreateCharacterClassButton> PressedEvent;

	public void Init(ClassModel classModel)
	{
		ClassModel = classModel;

		_textureRect.SetTexture(classModel.PortraitTexture);
		_colorOutline.SetModulate(classModel.PrimaryColor);
		_iconTexture.SetModulate(classModel.PrimaryColor);
		_iconTexture.SetTexture(classModel.IconTexture);
		_iconShadowTexture.SetTexture(classModel.IconTexture);

		this.DelayedCall(() =>
		{
			_container.PivotOffset = _container.Size * 0.5f;
		});

		_selected = true;
		_inactiveOverlay.TweenModulateAlpha(0f, 0f).Play();

		BetterButton.SetEnabled(true, false);

		BetterButton.Pressed += OnPressed;
	}

	public void SetSelected(bool active, bool canPress)
	{
		BetterButton.SetEnabled(canPress, false);

		if(_selected == active)
		{
			return;
		}

		_selected = active;

		_scaleTween?.Kill();
		if(_selected)
		{
			_scaleTween = GTweenSequenceBuilder.New()
				.AppendTime(0.05f)
				.Append(_container.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack))
				.Join(_inactiveOverlay.TweenModulateAlpha(0f, 0.15f))
				.Build().Play();
		}
		else
		{
			_scaleTween = GTweenSequenceBuilder.New()
				.AppendTime(0.05f)
				.Append(_container.TweenScale(0.9f, 0.15f).SetEasing(Easing.InBack))
				.Join(_inactiveOverlay.TweenModulateAlpha(1f, 0.15f))
				.Build().Play();
		}
	}

	private void OnPressed()
	{
		PressedEvent?.Invoke(this);
	}
}
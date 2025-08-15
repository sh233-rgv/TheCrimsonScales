using System;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public abstract partial class EffectButtonBase : Control
{
	public static readonly Color MandatoryColor = Color.FromHtml("ff3232");

	[Export]
	private BetterButton _button;
	[Export]
	private Panel _panel;

	private bool _pressed;

	public Effect Effect { get; set; }

	public event Action<EffectButtonBase> PressedEvent;

	public void Init(Effect effect)
	{
		Effect = effect;

		Init(Effect.Subscription.EffectButtonParameters);

		_button.Pressed += OnPressed;
		_button.MouseEntered += OnMouseEntered;
		_button.MouseExited += OnMouseExited;

		Scale = Vector2.Zero;
		this.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack).Play();

		if(effect.EffectType == EffectType.SelectableMandatory)
		{
			_panel.SelfModulate = MandatoryColor;
		}
	}

	protected abstract void Init(EffectButtonParameters parameters);

	public void Destroy()
	{
		Reparent(GetParent().GetParent());

		this.TweenScale(0f, 0.15f).SetEasing(Easing.InBack).OnComplete(QueueFree).Play();
	}

	private void OnPressed()
	{
		_pressed = true;

		PressedEvent?.Invoke(this);
	}

	private void OnMouseEntered()
	{
		if(!_pressed)
		{
			GameController.Instance.EffectInfoViewManager.CreateInfoView(this, Effect.Subscription.EffectInfoViewParameters);
		}
	}

	private void OnMouseExited()
	{
		GameController.Instance.EffectInfoViewManager.RemoveInfoView(this);
	}
}
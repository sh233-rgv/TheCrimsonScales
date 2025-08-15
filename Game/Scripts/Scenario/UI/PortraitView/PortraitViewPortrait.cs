using System;
using Godot;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public abstract partial class PortraitViewPortrait : Control
{
	[Export]
	private Control _container;
	[Export]
	protected TextureRect _portraitTexture;
	[Export]
	protected Label _initiativeLabel;
	[Export]
	private BetterButton _betterButton;
	[Export]
	private Control _selectedIndicator;

	//private bool _hasMoved;
	private GTween _tween;

	//public ITurnTakerGroup TurnTaker { get; private set; }
	public bool Selected { get; private set; }
	public abstract Initiative Initiative { get; }

	private Action<PortraitViewPortrait> _pressedEventInternal;

	public event Action<PortraitViewPortrait> PressedEvent
	{
		add
		{
			_betterButton.SetEnabled(true, true);
			_pressedEventInternal += value;
		}
		remove
		{
			_pressedEventInternal -= value;
			if(_pressedEventInternal == null)
			{
				_betterButton.SetEnabled(false, false);
			}
		}
	}

	public void Init()
	{
		_betterButton.Pressed += OnPressed;

		_selectedIndicator.Scale = Vector2.Zero;
		Scale = Vector2.Zero;
		this.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardable();

		_betterButton.SetEnabled(false, false);

		GameController.Instance.Map.TurnTakerChangedEvent += OnTurnTakerChanged;
	}

	public virtual void Destroy()
	{
		this.TweenScale(0f, 0.3f).SetEasing(Easing.InBack).OnComplete(QueueFree).PlayFastForwardable();

		GameController.Instance.Map.TurnTakerChangedEvent -= OnTurnTakerChanged;
	}

	public void Move(Vector2 position)
	{
		_tween?.Kill();

		_tween = this.TweenPosition(position, 0.2f).SetEasing(Easing.OutBack).PlayFastForwardable();

		// if(!_hasMoved)
		// {
		// 	_tween.Complete();
		// 	_hasMoved = true;
		// }
	}

	public void SetSelected(bool selected)
	{
		if(Selected == selected)
		{
			return;
		}

		Selected = selected;

		_container.TweenPositionY(selected ? 30f : 0f, 0.1f).SetEasing(Easing.OutBack).PlayFastForwardable();

		if(Selected)
		{
			_selectedIndicator.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack).PlayFastForwardable();
		}
		else
		{
			_selectedIndicator.TweenScale(0f, 0.15f).SetEasing(Easing.InBack).PlayFastForwardable();
		}
	}

	private void OnPressed()
	{
		_pressedEventInternal?.Invoke(this);
	}

	protected virtual void OnTurnTakerChanged(Figure figure)
	{
	}
}
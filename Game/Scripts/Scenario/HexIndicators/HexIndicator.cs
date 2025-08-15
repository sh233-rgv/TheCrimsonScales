using System;
using Godot;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class HexIndicator : Node2D
{
	[Export]
	private Node2D _selectedContainer;
	[Export]
	private Node2D _mandatoryContainer;
	[Export]
	private WorldButton _worldButton;

	private HexIndicatorType? _indicatorType;
	private GTween _selectedContainerTween;
	private GTween _mandatoryContainerTween;

	public Hex Hex { get; private set; }

	private event Action<HexIndicator> _pressedEvent;
	private event Action<HexIndicator, Vector2, Vector2> _draggedEvent;
	private event Action<HexIndicator, Vector2> _dragEndEvent;

	public void Init(Hex hex, bool canDrag)
	{
		Hex = hex;

		GlobalPosition = hex.GlobalPosition;
		Scale = Vector2.Zero;
		this.TweenScale(Vector2.One, 0.15f).SetEasing(Easing.OutBack).Play();

		_worldButton.SetCanDrag(canDrag);

		_selectedContainer.Scale = Vector2.Zero;
		_mandatoryContainer.Scale = Vector2.Zero;

		_worldButton.PressedEvent += OnPressed;
		_worldButton.DraggedEvent += OnDragged;
		_worldButton.DrageEndEvent += OnDragEnd;
	}

	public void Destroy()
	{
		_worldButton.PressedEvent -= OnPressed;
		_worldButton.DraggedEvent -= OnDragged;

		this.TweenScale(Vector2.Zero, 0.15f).SetEasing(Easing.InBack).OnComplete(QueueFree).Play();
	}

	public void SetType(HexIndicatorType indicatorType, Action<HexIndicator> onPressed,
		Action<HexIndicator, Vector2, Vector2> onDragged = null, Action<HexIndicator, Vector2> onDragEnd = null)
	{
		if(indicatorType == _indicatorType)
		{
			return;
		}

		HexIndicatorType? previousIndicatorType = _indicatorType;
		_indicatorType = indicatorType;

		// _selectedContainerTween?.Kill();
		// _selectedContainerTween = null;
		// _mandatoryContainerTween?.Kill();
		// _mandatoryContainerTween = null;

		switch(_indicatorType)
		{
			case HexIndicatorType.Selected:
				_selectedContainerTween = _selectedContainer.TweenScale(Vector2.One, 0.2f).SetEasing(Easing.OutBack);
				_selectedContainerTween.Play();
				break;
			case HexIndicatorType.Mandatory:
				_mandatoryContainerTween = _mandatoryContainer.TweenScale(Vector2.One, 0.2f).SetEasing(Easing.OutBack);
				_mandatoryContainerTween.Play();
				break;
		}

		switch(previousIndicatorType)
		{
			case HexIndicatorType.Selected:
				_selectedContainerTween = _selectedContainer.TweenScale(Vector2.Zero, 0.2f).SetEasing(Easing.InBack);
				_selectedContainerTween.Play();
				break;
			case HexIndicatorType.Mandatory:
				_mandatoryContainerTween = _mandatoryContainer.TweenScale(Vector2.Zero, 0.2f).SetEasing(Easing.InBack);
				_mandatoryContainerTween.Play();
				break;
		}

		// if(_indicatorType == HexIndicatorType.Selected)
		// {
		// 	_selectedContainerTween = _selectedContainer.TweenScale(Vector2.One, 0.2f).SetEasing(Easing.OutBack);
		// 	_selectedContainerTween.Play();
		// }
		// else if(previousIndicatorType == HexIndicatorType.Selected)
		// {
		// 	_selectedContainerTween = _selectedContainer.TweenScale(Vector2.Zero, 0.2f).SetEasing(Easing.InBack);
		// 	_selectedContainerTween.Play();
		// }

		_pressedEvent = onPressed;
		_draggedEvent = onDragged;
		_dragEndEvent = onDragEnd;
	}

	private void OnPressed()
	{
		_pressedEvent?.Invoke(this);

		if(_pressedEvent != null)
		{
			AppController.Instance.AudioController.Play(SFX.Click, 3f, 3.2f, volumeDb: -8);
			VibrationController.Vibrate();
		}
	}

	private void OnDragged(Vector2 previousPosition, Vector2 currentPosition)
	{
		_draggedEvent?.Invoke(this, previousPosition, currentPosition);

		// if(_draggedEvent != null)
		// {
		// 	AppController.Instance.AudioController.Play(SFX.Click, 3f, 3.2f, volumeDb: -8);
		// 	VibrationController.Vibrate();
		// }
	}

	private void OnDragEnd(Vector2 position)
	{
		_dragEndEvent?.Invoke(this, position);

		if(_dragEndEvent != null)
		{
			AppController.Instance.AudioController.Play(SFX.Click, 2.5f, 2.7f, volumeDb: -8);
			VibrationController.Vibrate();
		}
	}
}
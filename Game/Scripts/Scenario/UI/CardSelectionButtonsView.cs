using System;
using System.Collections.Generic;
using Godot;

public partial class CardSelectionButtonsView : Control
{
	[Export]
	private ChoiceButton _startRoundButton;

	private readonly List<object> _blockers = new List<object>();

	private bool _startActive;

	private event Action StartPressedEvent;

	public override void _Ready()
	{
		base._Ready();

		_startRoundButton.BetterButton.Pressed += OnStartRoundPressed;
	}

	public void Open(Action onStartPressed)
	{
		StartPressedEvent = onStartPressed;

		_startRoundButton.SetActive(false);
	}

	public void SetButtons(bool startActive)
	{
		_startActive = startActive;

		UpdateButtons();
	}

	public void Close()
	{
		_startActive = false;

		UpdateButtons();
	}

	public void Block(object blocker)
	{
		_blockers.AddIfNew(blocker);

		UpdateButtons();
	}

	public void UnBlock(object blocker)
	{
		_blockers.Remove(blocker);

		UpdateButtons();
	}

	private void UpdateButtons()
	{
		_startRoundButton.SetActive(_startActive && _blockers.Count == 0);
	}

	private void OnStartRoundPressed()
	{
		StartPressedEvent?.Invoke();
	}
}
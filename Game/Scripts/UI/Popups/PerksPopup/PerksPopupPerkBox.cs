using System;
using Godot;

public partial class PerksPopupPerkBox : Control
{
	[Export]
	private BetterButton _button;
	[Export]
	private Control _checkmark;

	public int PerkIndex { get; private set; }

	public event Action<PerksPopupPerkBox> PressedEvent;

	public override void _Ready()
	{
		base._Ready();

		_button.Pressed += OnPressed;
	}

	public void Init(int perkIndex)
	{
		PerkIndex = perkIndex;
	}

	public void SetAcquired(bool acquired)
	{
		_checkmark.SetVisible(acquired);
	}

	public void SetCanPress(bool canPress)
	{
		_button.SetEnabled(canPress);
	}

	private void OnPressed()
	{
		PressedEvent?.Invoke(this);
	}
}
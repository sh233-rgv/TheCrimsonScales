using Godot;

public partial class PerksPopupPerkBox : Control
{
	[Export]
	private BetterButton _button;
	[Export]
	private Control _checkmark;

	public int PerkIndex { get; private set; }

	public override void _Ready()
	{
		base._Ready();

		_button.Pressed += OnPressed;
	}

	public void Init(int perkIndex, bool acquiredPerk)
	{
		PerkIndex = perkIndex;

		_checkmark.SetVisible(acquiredPerk);
	}

	private void OnPressed()
	{
	}
}
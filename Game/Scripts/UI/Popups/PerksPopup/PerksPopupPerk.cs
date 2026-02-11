using Godot;

public partial class PerksPopupPerk : Control
{
	// [Export]
	// private BetterButton _button;
	[Export]
	private RichTextLabel _description;

	public PerkModel PerkModel { get; private set; }
	public int PerkIndex { get; private set; }
	public bool Acquired { get; private set; }

	public override void _Ready()
	{
		base._Ready();

		//_button.Pressed += OnPressed;
	}

	public void Init(PerkModel perkModel, int perkIndex, bool acquired)
	{
		_description.SetText(perkModel.GetType().Name);
	}

	private void OnPressed()
	{
	}
}
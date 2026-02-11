using System.Collections.Generic;
using Godot;

public partial class PerksPopupPerk : Control
{
	[Export]
	private PackedScene _perkBoxScene;
	[Export]
	private Control _perkBoxContainer;

	[Export]
	private RichTextLabel _description;

	private readonly List<PerksPopupPerkBox> _perkBoxes = new List<PerksPopupPerkBox>();

	public PerkModel PerkModel { get; private set; }
	public int PerkIndex { get; private set; }
	public bool Acquired { get; private set; }

	public void Init(PerkModel perkModel, int startingPerkIndex, int perkCount, SavedCharacter savedCharacter)
	{
		PerkModel = perkModel;

		for(int i = 0; i < perkCount; i++)
		{
			int perkIndex = startingPerkIndex + i;
			PerksPopupPerkBox perkBox = _perkBoxScene.Instantiate<PerksPopupPerkBox>();
			_perkBoxContainer.AddChild(perkBox);
			perkBox.Init(perkIndex, savedCharacter.AcquiredPerkIndices.Contains(perkIndex));
			_perkBoxes.Add(perkBox);
		}

		_description.SetText(perkModel.GetType().Name);
	}

	private void OnPressed()
	{
	}
}
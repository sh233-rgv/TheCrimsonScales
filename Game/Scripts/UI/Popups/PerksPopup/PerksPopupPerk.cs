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

	private SavedCharacter _savedCharacter;

	private readonly List<PerksPopupPerkBox> _perkBoxes = new List<PerksPopupPerkBox>();

	public PerkModel PerkModel { get; private set; }
	public int StartingPerkIndex { get; private set; }
	public int PerkCount { get; private set; }
	public bool Acquired { get; private set; }

	public void Init(PerkModel perkModel, int startingPerkIndex, int perkCount, SavedCharacter savedCharacter)
	{
		PerkModel = perkModel;
		StartingPerkIndex = startingPerkIndex;
		PerkCount = perkCount;
		_savedCharacter = savedCharacter;

		for(int i = 0; i < perkCount; i++)
		{
			int perkIndex = startingPerkIndex + i;
			PerksPopupPerkBox perkBox = _perkBoxScene.Instantiate<PerksPopupPerkBox>();
			_perkBoxContainer.AddChild(perkBox);
			perkBox.Init(perkIndex);
			perkBox.PressedEvent += OnPerkBoxPressed;
			_perkBoxes.Add(perkBox);
		}

		UpdatePerks();

		_description.SetText(perkModel.ToString(_description.GetRichTextParameters()));

		_savedCharacter.PerksChangedEvent += OnPerksChangedEvent;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		if(_savedCharacter != null)
		{
			_savedCharacter.PerksChangedEvent -= OnPerksChangedEvent;
		}
	}

	private void UpdatePerks()
	{
		for(int i = 0; i < PerkCount; i++)
		{
			int perkIndex = StartingPerkIndex + i;
			PerksPopupPerkBox perkBox = _perkBoxes[i];
			perkBox.SetAcquired(_savedCharacter.GetPerkAcquired(perkIndex));
		}
	}

	private void OnPerksChangedEvent(SavedCharacter savedCharacter)
	{
		UpdatePerks();
	}

	private void OnPerkBoxPressed(PerksPopupPerkBox perkBox)
	{
		AppController.Instance.PopupManager.OpenPopupOnTop(new PerkConfirmationPopup.Request()
		{
			SavedCharacter = _savedCharacter,
			PerkModel = PerkModel,
			PerkIndex = perkBox.PerkIndex
		});
	}
}
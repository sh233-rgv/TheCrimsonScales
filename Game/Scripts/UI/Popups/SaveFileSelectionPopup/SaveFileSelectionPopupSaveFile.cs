using System.Collections.Generic;
using Godot;

public partial class SaveFileSelectionPopupSaveFile : Control
{
	[Export]
	private Label _numberLabel;

	[Export]
	private Control _emptyContainer;
	[Export]
	private Control _saveFileContainer;

	[Export]
	private PackedScene _characterScene;
	[Export]
	private Control _characterParent;

	private readonly List<SaveFileSelectionPopupSaveFileCharacter> _characters = new List<SaveFileSelectionPopupSaveFileCharacter>();

	public void Init(int index, CampaignSaveData campaignSaveData)
	{
		_numberLabel.SetText(index.ToString());

		SavedCampaign savedCampaign = campaignSaveData.SavedCampaign;

		_emptyContainer.SetVisible(savedCampaign == null);
		_saveFileContainer.SetVisible(savedCampaign != null);

		if(savedCampaign != null)
		{
			foreach(SavedCharacter savedCharacter in savedCampaign.Characters)
			{
				SaveFileSelectionPopupSaveFileCharacter character = _characterScene.Instantiate<SaveFileSelectionPopupSaveFileCharacter>();
				_characterParent.AddChild(character);
				character.Init(savedCharacter);
				_characters.Add(character);
			}
		}
	}
}
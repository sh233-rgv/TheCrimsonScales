using System;
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

	[Export]
	private Label _dateLabel;
	[Export]
	private Label _locationLabel;
	[Export]
	private Label _difficultyLabel;

	[Export]
	private BetterButton _newCampaignButton;
	[Export]
	private BetterButton _loadButton;

	private readonly List<SaveFileSelectionPopupSaveFileCharacter> _characters = new List<SaveFileSelectionPopupSaveFileCharacter>();

	private int _index;

	public override void _Ready()
	{
		base._Ready();

		_newCampaignButton.Pressed += OnNewCampaignPressed;
		_loadButton.Pressed += OnLoadPressed;
	}

	public void Init(int index, CampaignSaveData campaignSaveData)
	{
		_index = index;

		_numberLabel.SetText(_index.ToString());

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

			_dateLabel.SetText(campaignSaveData.LastSaved == null ? "N.A." : campaignSaveData.LastSaved.Value.ToString("yyyy/MM/dd"));
			_locationLabel.SetText(
				campaignSaveData.SavedCampaign.SavedScenario == null
					? "Gloomhaven"
					: $"Scenario {ModelDB.GetById<ScenarioModel>(campaignSaveData.SavedCampaign.SavedScenario.ScenarioModelId).ScenarioNumber}");
			_difficultyLabel.SetText(DifficultySliderOptionView.DifficultyToString(campaignSaveData.Options.Difficulty.Value));
		}
	}

	private void OnNewCampaignPressed()
	{
		AppController.Instance.SceneLoader.RequestSceneChange(new NewCampaignSceneRequest(_index));
	}

	private void OnLoadPressed()
	{
		AppController.Instance.SaveManager.SetCampaignIndex(_index);
		AppController.Instance.DeviceSaveData.LastCampaignIndex = _index;

		AppController.Instance.SaveManager.SaveAll();

		AppController.Instance.SceneLoader.RequestSceneChange(
			new BetweenScenariosSceneRequest(AppController.Instance.CampaignSaveData.SavedCampaign));
	}
}
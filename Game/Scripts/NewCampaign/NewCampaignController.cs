using Godot;

public partial class NewCampaignController : SceneController<NewCampaignController>
{
	private NewCampaignSceneRequest _sceneRequest;

	public override void _EnterTree()
	{
		_sceneRequest = AppController.Instance.SceneLoader.CurrentSceneRequest as NewCampaignSceneRequest;

		if(_sceneRequest == null)
		{
			_sceneRequest = new NewCampaignSceneRequest();
		}
	}

	public override void _Ready()
	{
		base._Ready();

		AppController.Instance.AudioController.SetBGM("res://Audio/BGM/Call to Adventure FULL LOOP TomMusic.ogg");
		AppController.Instance.AudioController.SetBGS(null);
	}

	private void OnContinuePressed()
	{
		SavedCampaign savedCampaign = AppController.Instance.SaveFile.SaveData.SavedCampaign;
		if(savedCampaign.SavedScenario == null)
		{
			AppController.Instance.SceneLoader.RequestSceneChange(new BetweenScenariosSceneRequest(savedCampaign));
		}
		else
		{
			AppController.Instance.SceneLoader.RequestSceneChange(new GameSceneRequest(savedCampaign));
		}
	}

	private void OnNewGamePressed()
	{
		if(AppController.Instance.SaveFile.SaveData.SavedCampaign == null)
		{
			StartNewCampaign();
		}
		else
		{
			AppController.Instance.PopupManager.OpenPopupOnTop(new TextPopup.Request("Are you sure?",
				"Are you sure you want to start a new campaign?\nThis will overwrite your saved campaign and can not be undone!",
				new TextButton.Parameters("Cancel",
					() =>
					{
					}
				),
				new TextButton.Parameters("New Campaign",
					() =>
					{
						StartNewCampaign();
					},
					TextButton.ColorType.Red,
					width: 300
				)
			));
		}
	}

	private void OnOptionsPressed()
	{
		AppController.Instance.PopupManager.RequestPopup(new OptionsPopup.Request());
	}

	private void StartNewCampaign()
	{
		AppController.Instance.SceneLoader.RequestSceneChange(new NewCampaignSceneRequest());
		//AppController.Instance.PopupManager.RequestPopup(new CreateCampaignPopup.Request());
	}

	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}
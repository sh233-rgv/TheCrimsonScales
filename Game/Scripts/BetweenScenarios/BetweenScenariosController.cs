using System;
using System.Linq;
using Fractural.Tasks;
using Godot;

public partial class BetweenScenariosController : SceneController<BetweenScenariosController>
{
	[Export]
	public BetweenScenariosSidePanel BetweenScenariosSidePanel { get; private set; }

	[Export]
	public BetweenScenariosCharacterPortraitManager CharacterPortraitManager { get; private set; }

	[Export]
	public ScenarioFlowchart ScenarioFlowchart { get; private set; }

	[Export]
	public EventOverlay EventOverlay { get; private set; }

	public BetweenScenariosSceneRequest SceneRequest { get; private set; }

	public RandomNumberGenerator RNG { get; private set; }

	public SavedCampaign SavedCampaign => SceneRequest.SavedCampaign;

	public override void _EnterTree()
	{
		base._EnterTree();

		SceneRequest = AppController.Instance.SceneLoader.CurrentSceneRequest as BetweenScenariosSceneRequest;

		if(SceneRequest == null)
		{
			SceneRequest = new BetweenScenariosSceneRequest(SavedCampaign.Test());
		}

		if(SceneRequest.SavedCampaign.Characters.Count == 0)
		{
			this.DelayedCall(() =>
			{
				AppController.Instance.PopupManager.RequestPopup(new TextPopup.Request("Welcome!",
					"Welcome to the very early access version of The Crimson Scales!\nPlease create a couple of characters to get started on this campaign. " +
					"You can do so using the button in the bottom-left corner."
				));
			}, 0.5f);
		}

		if(
			SceneRequest.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(ModelDB.Scenario<Scenario010>()).Completed &&
			SceneRequest.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(ModelDB.Scenario<Scenario013>()).Completed &&
			SceneRequest.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(ModelDB.Scenario<Scenario014>()).Completed)
		{
			this.DelayedCall(() =>
			{
				AppController.Instance.PopupManager.RequestPopup(new TextPopup.Request("End of Demo",
					"Thank you for playing this demo of The Crimson Scales!\nHope you had fun!" +
					"\nAny and all feedback is very welcome. Please do not hesitate to let us know your thoughts."
				));
			});
		}

		RNG = new RandomNumberGenerator();
		RNG.Randomize();

		AppController.Instance.AudioController.SetBGM("res://Audio/BGM/old-tavern-cinematic-atmosphere-fairytale-273871.mp3");
		AppController.Instance.AudioController.SetBGS(null);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if(@event is InputEventKey inputEventKey && inputEventKey.Pressed)
		{
			if(inputEventKey.Keycode == Key.Escape)
			{
				OpenMenuPopup();
			}

			if(inputEventKey.Keycode == Key.X && OS.IsDebugBuild())
			{
				foreach(SavedCharacter savedCharacter in SavedCampaign.Characters)
				{
					savedCharacter.AddXP(30);
				}
			}
		}
	}

	public override void _Notification(int what)
	{
		base._Notification(what);

		if(what == NotificationWMGoBackRequest)
		{
			OpenMenuPopup();
		}
	}

	public void TryStartScenario(ScenarioModel scenarioModel)
	{
		if(SavedCampaign.Characters.Count < 2)
		{
			AppController.Instance.PopupManager.RequestPopup(new TextPopup.Request("Cannot start scenario",
				"You need at least 2 characters in order to start a scenario.\n"));

			return;
		}

		AppController.Instance.PopupManager.OpenPopupOnTop(new TextPopup.Request($"Scenario {scenarioModel.ScenarioNumber}",
			$"Start scenario {scenarioModel.ScenarioNumber}?",
			new TextButton.Parameters("Cancel",
				() =>
				{
				}
			),
			new TextButton.Parameters("Confirm",
				() =>
				{
					StartScenarioSequence(scenarioModel).Forget();
				},
				TextButton.ColorType.Green
			)
		));
	}

	private async GDTaskVoid StartScenarioSequence(ScenarioModel scenarioModel)
	{
		await EventOverlay.DrawEventCard(EventType.Road);

		SavedCampaign savedCampaign = SavedCampaign;
		float characterLevelSum = savedCampaign.Characters.Sum(character => character.Level);
		int scenarioLevel =
			Mathf.CeilToInt((characterLevelSum / savedCampaign.Characters.Count) / 2f) +
			AppController.Instance.SaveFile.SaveData.Options.Difficulty.Value;
		scenarioLevel = Mathf.Clamp(scenarioLevel, 0, 7);
		savedCampaign.SavedScenario = new SavedScenario()
		{
			Id = Guid.NewGuid(),
			AppVersion = AppController.Instance.SaveFile.SaveData.AppVersion,
			ScenarioModelId = scenarioModel.Id.ToString(),
			Seed = GD.RandRange(0, int.MaxValue),
			ScenarioLevel = scenarioLevel,
			IsOnline = false
		};

		AppController.Instance.SaveFile.SaveData.SavedCampaign = savedCampaign;
		AppController.Instance.SceneLoader.RequestSceneChange(new GameSceneRequest(savedCampaign));
	}

	private void OpenMenuPopup()
	{
		this.DelayedCall(() =>
		{
			if(!AppController.Instance.PopupManager.IsPopupOpen())
			{
				AppController.Instance.PopupManager.RequestPopup(new MenuPopup.Request());
			}
		});
	}
}
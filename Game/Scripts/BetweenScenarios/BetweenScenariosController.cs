using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

	[Export]
	public BetweenScenariosActionManager ActionManager { get; private set; }

	[Export]
	public ItemShop ItemShop { get; private set; }

	[Export]
	public CharacterCreationOverlay CharacterCreationOverlay { get; private set; }

	[Export]
	public BetweenScenariosClassUnlockOverlay UnlockOverlay { get; private set; }

	private readonly List<Reward> _duringDowntimeEventRewards = new List<Reward>();

	public BetweenScenariosSceneRequest SceneRequest { get; private set; }

	public RandomNumberGenerator RNG { get; private set; }

	public BetweenScenariosEvents Events { get; private set; }

	public SavedCampaign SavedCampaign => SceneRequest.SavedCampaign;

	public override void _EnterTree()
	{
		base._EnterTree();

		SceneRequest = AppController.Instance.SceneLoader.CurrentSceneRequest as BetweenScenariosSceneRequest;

		if(SceneRequest == null)
		{
			SceneRequest = new BetweenScenariosSceneRequest(SavedCampaign.Test());
		}

		RNG = new RandomNumberGenerator();
		RNG.Randomize();

		Events = new BetweenScenariosEvents();

		AppController.Instance.AudioController.SetBGM("res://Audio/BGM/old-tavern-cinematic-atmosphere-fairytale-273871.mp3");
		AppController.Instance.AudioController.SetBGS(null);

		SavedCampaign.CharactersChangedEvent += OnCharactersChanged;
		SubscribeCharacters();
	}

	public override void _Ready()
	{
		base._Ready();

		StartSequence().Forget();
	}

	public override void _ExitTree()
	{
		for(int i = _duringDowntimeEventRewards.Count - 1; i >= 0; i--)
		{
			UnsubscribeDuringDowntime(_duringDowntimeEventRewards[i]);
		}

		if(SavedCampaign != null)
		{
			foreach(SavedCharacter savedCharacter in SavedCampaign.Characters)
			{
				savedCharacter.SavedPersonalQuest?.Model.OnBetweenScenariosEnded(savedCharacter);
			}

			foreach(SavedPartyGoal savedPartyGoal in SavedCampaign.SavedPartyGoals.PartyGoals)
			{
				savedPartyGoal.Model.OnBetweenScenariosEnded(savedPartyGoal);
			}
		}

		UnsubscribeCharacters();

		base._ExitTree();
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

			if(OS.IsDebugBuild())
			{
				if(inputEventKey.Keycode == Key.X)
				{
					foreach(SavedCharacter savedCharacter in SavedCampaign.Characters)
					{
						savedCharacter.AddXP(30);
					}
				}

				if(inputEventKey.Keycode == Key.C)
				{
					foreach(SavedCharacter savedCharacter in SavedCampaign.Characters)
					{
						savedCharacter.AddCheckmark();
					}
				}

				if(inputEventKey.Keycode == Key.P)
				{
					SavedCampaign.AdjustProsperity(1);
				}

				if(inputEventKey.Keycode == Key.R)
				{
					if(Input.IsKeyPressed(Key.Shift))
					{
						SavedCampaign.AdjustReputation(-1);
					}
					else
					{
						SavedCampaign.AdjustReputation(1);
					}
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
				"You need at least 2 characters in order to start a scenario."));

			return;
		}

		if(SavedCampaign.Characters.Any(character => character.GetCanRetire(SavedCampaign)))
		{
			AppController.Instance.PopupManager.RequestPopup(new TextPopup.Request("Cannot start scenario",
				"One of your characters is ready to retire."));

			return;
		}

		if(SavedCampaign.Characters.Any(character => character.CheckCanLevelUp() || character.LevelUpInProgress))
		{
			AppController.Instance.PopupManager.RequestPopup(new TextPopup.Request("Cannot start scenario",
				"One of your characters is ready to level up."));

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

	public void RetireCharacter(SavedCharacter savedCharacter, SavedCampaign savedCampaign)
	{
		AppController.Instance.PopupManager.RequestPopup(new RetirementPopup.Request()
		{
			Character = savedCharacter,
			SavedCampaign = savedCampaign,
			UnlockedClass = savedCampaign.GetUnlockedClass(savedCharacter)
		});

		savedCampaign.RetireCharacter(savedCharacter);
	}

	public void UnsubscribeDuringDowntime(Reward reward)
	{
		reward.UnsubscribeDuringDowntime();
		_duringDowntimeEventRewards.Remove(reward);
	}

	private async GDTaskVoid StartSequence()
	{
		CancellationToken cancellationToken = DestroyCancellationToken;

		const string introductionSeenKey = "IntroductionSeen";
		if(!SavedCampaign.GetCustomValue<bool>(introductionSeenKey))
		{
			await AppController.Instance.StoryView.OpenAsync("Introduction", null,
				"""
				Yet again you find yourselves in a tavern, but this time in a part of town most sane folk wouldn’t dream of entering. You do not frequent this area regularly, but truth be told, business has been slow and this inn is even cheaper and down-market than your normal haunts.

				Despite being in this part of town, your reputation has clearly gone before you, as it doesn’t take long before a particularly shady-looking character sidles up to you. His hood half-obscures a dirty, scarred face, his grubby cloak seems to be heavily stained with dried blood, and he twitches lightly as he starts to speak.

				“I hear you guys are open to adventure,” he mutters furtively, in a strangely-high voice. You shrug noncommittally, unsure where this is going. “I can make you rich and grant great influence if you can acquire a small trinket on my behalf?” Still dubious, you merely raise an eyebrow. But that, it seems, is enough.

				“Very good. Go to the shore of the Dark Lake, near the Watcher Mountains. There are rumors of some strange creatures there which may lead you to what you are looking for. If you are successful, I will make the necessary introductions.” Simultaneously intrigued and confused, you can’t help but ask more questions. “What is it we’re actually looking for? And what do we do when we’ve got it?” The hooded stranger half smiles. “You don’t need to know what it is. You’ll either find it, or you won’t. Look for me here if you are successful...”

				With that, the cloaked figure turns on his heel and sweeps out of the bar. You look at each other and shrug. You had no plans tomorrow anyway.
				""",
				fadeInDuration: 0f, cancellationToken: cancellationToken);

			SavedCampaign.SetCustomValue(introductionSeenKey, true);
		}

		await GDTask.Yield(cancellationToken);
		await GDTask.Delay(0.2f, cancellationToken: cancellationToken);

		foreach(SavedCharacter savedCharacter in SavedCampaign.Characters)
		{
			if(savedCharacter.SavedPersonalQuest != null)
			{
				await savedCharacter.SavedPersonalQuest.Model.OnBetweenScenariosStarted(savedCharacter);
			}
		}

		foreach(SavedPartyGoal savedPartyGoal in SavedCampaign.SavedPartyGoals.PartyGoals)
		{
			await savedPartyGoal.Model.OnBetweenScenariosStarted(savedPartyGoal);
		}

		if(SavedCampaign.SavedEvents.CanDrawCityEvent && SavedCampaign.SavedEvents.CityEventDeckIds.Count > 0)
		{
			await EventOverlay.DrawEventCard(EventType.City, cancellationToken);
		}

		foreach(SavedEventState savedEventState in SavedCampaign.SavedEvents.SavedEventStates)
		{
			foreach(Reward eventReward in savedEventState.Choice.GetRewards(savedEventState))
			{
				if(eventReward.Type == RewardType.DuringDowntime)
				{
					eventReward.SubscribeDuringDowntime(savedEventState);

					_duringDowntimeEventRewards.Add(eventReward);
				}
			}
		}

		if(SceneRequest.SavedCampaign.Characters.Count == 0)
		{
			AppController.Instance.PopupManager.RequestPopup(new TextPopup.Request("Welcome!",
				"""
				Welcome to the very early access version of The Crimson Scales!\nPlease create a couple of characters to get started on this campaign.

				You can do so using the button in the bottom-left corner."
				"""
			));
		}

		// if(
		// 	SceneRequest.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(ModelDB.Scenario<Scenario010>()).Completed &&
		// 	SceneRequest.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(ModelDB.Scenario<Scenario013>()).Completed &&
		// 	SceneRequest.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(ModelDB.Scenario<Scenario014>()).Completed)
		// {
		// 	AppController.Instance.PopupManager.RequestPopup(new TextPopup.Request("End of Demo",
		// 		"Thank you for playing this demo of The Crimson Scales!\nHope you had fun!" +
		// 		"\nAny and all feedback is very welcome. Please do not hesitate to let us know your thoughts."
		// 	));
		// }

		ActionManager.Init();
	}

	private async GDTaskVoid StartScenarioSequence(ScenarioModel scenarioModel)
	{
		CancellationToken cancellationToken = DestroyCancellationToken;

		BetweenScenariosEvents.DrawRoadEvent.Parameters drawRoadEventParameters =
			BetweenScenariosEvents.DrawRoadEventEvent.Fire(
				new BetweenScenariosEvents.DrawRoadEvent.Parameters());

		if(drawRoadEventParameters.DrawEvent)
		{
			await EventOverlay.DrawEventCard(EventType.Road, cancellationToken);
		}

		SavedCampaign savedCampaign = SavedCampaign;
		float characterLevelSum = savedCampaign.Characters.Sum(character => character.Level);
		int scenarioLevel =
			Mathf.CeilToInt((characterLevelSum / savedCampaign.Characters.Count) / 2f) +
			AppController.Instance.SaveFile.SaveData.Options.Difficulty.Value;
		scenarioLevel = Mathf.Clamp(scenarioLevel, 0, 7);
		savedCampaign.SetSavedScenario(new SavedScenario()
		{
			Id = Guid.NewGuid(),
			AppVersion = AppController.Instance.SaveFile.SaveData.AppVersion,
			ScenarioModelId = scenarioModel.Id.ToString(),
			Seed = GD.RandRange(0, int.MaxValue),
			ScenarioLevel = scenarioLevel,
			IsOnline = false
		});

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

	private void SubscribeCharacters()
	{
		foreach(SavedCharacter savedCharacter in SavedCampaign.AllCharacters)
		{
			savedCharacter.XPChangedEvent += OnXPChanged;
		}
	}

	private void UnsubscribeCharacters()
	{
		foreach(SavedCharacter savedCharacter in SavedCampaign.AllCharacters)
		{
			savedCharacter.XPChangedEvent -= OnXPChanged;
		}
	}

	private void OnCharactersChanged()
	{
		UnsubscribeCharacters();
		SubscribeCharacters();
	}

	private void OnXPChanged(SavedCharacter savedCharacter)
	{
		BetweenScenariosEvents.XPChangedEvent.Fire(new BetweenScenariosEvents.XPChanged.Parameters(savedCharacter));
	}
}
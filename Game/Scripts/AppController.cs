using System.Collections.Generic;
using System.Threading;
using Fractural.Tasks;
using Godot;

public partial class AppController : SingletonNode<AppController>
{
	[Export]
	public InputController InputController { get; private set; }

	[Export]
	public SceneLoader SceneLoader { get; private set; }

	[Export]
	public PopupManager PopupManager { get; private set; }

	[Export]
	public AudioController AudioController { get; private set; }

	[Export]
	public CardSelectionCardPreview CardSelectionCardPreview { get; private set; }

	[Export]
	public ItemPreview ItemPreview { get; private set; }

	[Export]
	public PersonalQuestProgressUpdateView PersonalQuestProgressUpdateView { get; private set; }

	[Export]
	public BattleGoalProgressUpdateView BattleGoalProgressUpdateView { get; private set; }

	[Export]
	public StoryView StoryView { get; private set; }

	// public SaveFile SaveFile { get; private set; }
	public SaveManager SaveManager { get; private set; }

	public DeviceSaveData DeviceSaveData => SaveManager.DeviceSaveFile.SaveData;
	public CampaignSaveData CampaignSaveData => SaveManager.CampaignSaveFile?.SaveData;

	public SavedDeviceOptions DeviceOptions => DeviceSaveData.Options;
	public SavedCampaignOptions CampaignOptions => CampaignSaveData?.Options;

	public override void _EnterTree()
	{
		SaveManager = new SaveManager();
	}

	public override void _Ready()
	{
		base._Ready();

		//TODO: Reimplement this warning when continuing with a campaign save file
// 		if(SaveFile.RemovedSavedScenario)
// 		{
// 			PopupManager.RequestPopup(new TextPopup.Request("New Version",
// 				"""
// 				A new version of The Crimson Scales was installed. This unfortunately meant that the progress on the last scenario was incompatible with the new version.
//
// 				Please always make sure to finish up a scenario before installing a new version of the application!
// 				"""));
// 		}

		DeviceOptions.FullScreen.ValueChangedEvent += OnFullScreenChanged;
		OnFullScreenChanged(DeviceOptions.FullScreen.Value);
	}

	public async GDTask GiveRewards(SavedCampaign savedCampaign, List<SavedReward> rewards, bool showPopup = true,
		CancellationToken cancellationToken = default)
	{
		if(showPopup)
		{
			PopupManager.RequestPopup(new RewardsPopup.Request()
			{
				Rewards = rewards,
			});

			await GDTask.WaitWhile(() => PopupManager.IsPopupOpen(), cancellationToken: cancellationToken);
		}

		foreach(SavedReward reward in rewards)
		{
			if(reward.Type == RewardType.Immediate)
			{
				await reward.ImmediateResolve(savedCampaign, cancellationToken);
			}
			else
			{
				savedCampaign.SavedRewards.AddReward(reward);

				if(BetweenScenariosController.Instance != null)
				{
					BetweenScenariosController.Instance.SubscribeDuringDowntime(reward);
				}
			}
		}
	}

	public void RetireCharacter(SavedCharacter savedCharacter, SavedCampaign savedCampaign, bool addRetirementEvents = true)
	{
		PopupManager.RequestPopup(new RetirementPopup.Request()
		{
			Character = savedCharacter,
			SavedCampaign = savedCampaign,
			UnlockedClass = savedCampaign.GetUnlockedClass(savedCharacter)
		});

		savedCampaign.RetireCharacter(savedCharacter, addRetirementEvents);
	}

	public void SaveGame()
	{
		SaveManager.SaveGame();
	}

	private void OnFullScreenChanged(bool fullScreen)
	{
		DisplayServer.WindowMode windowMode = DisplayServer.WindowGetMode();

		if(!fullScreen && windowMode == DisplayServer.WindowMode.Fullscreen)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}

		if(fullScreen && windowMode != DisplayServer.WindowMode.Fullscreen)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
	}
}
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

	public SaveFile SaveFile { get; private set; }

	public SavedOptions Options => SaveFile.SaveData.Options;

	public override void _EnterTree()
	{
		SaveFile = new SaveFile("user://SaveFile.save");
	}

	public override void _Ready()
	{
		base._Ready();

		if(SaveFile.RemovedSavedScenario)
		{
			PopupManager.RequestPopup(new TextPopup.Request("New Version",
				"""
				A new version of The Crimson Scales was installed. This unfortunately meant that the progress on the last scenario was incompatible with the new version.

				Please always make sure to finish up a scenario before installing a new version of the application!
				"""));
		}
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
		}
	}
}
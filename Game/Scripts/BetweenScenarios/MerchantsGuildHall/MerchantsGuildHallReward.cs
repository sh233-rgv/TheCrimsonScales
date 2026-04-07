using System.Collections.Generic;
using System.Threading;
using Fractural.Tasks;
using Godot;

public partial class MerchantsGuildHallReward : Control
{
	[Export]
	private BetterButton _button;
	[Export]
	private TextureRect _checkmark;
	[Export]
	private RichTextLabel _description;

	private SavedMerchantsGuildHallReward _savedReward;

	public override void _Ready()
	{
		base._Ready();

		_button.Pressed += OnPressed;
	}

	public void Init(SavedMerchantsGuildHallReward savedReward)
	{
		_savedReward = savedReward;

		RichTextParameters parameters = _description.GetRichTextParameters();
		_description.SetText(savedReward.Model.GetDescription(parameters));

		_savedReward.UnlockedEvent += OnUnlocked;

		UpdateVisuals();
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		if(_savedReward != null)
		{
			_savedReward.UnlockedEvent -= OnUnlocked;
		}
	}

	private void UpdateVisuals()
	{
		_checkmark.SetVisible(_savedReward.Unlocked);
	}

	private async GDTaskVoid GiveRewards()
	{
		CancellationToken cancellationToken = BetweenScenariosController.Instance.DestroyCancellationToken;

		List<Reward> rewards = _savedReward.Model.GetRewards();
		await AppController.Instance.GiveRewards(BetweenScenariosController.Instance.SavedCampaign, rewards, cancellationToken: cancellationToken);

		_savedReward.SetUnlocked();

		AppController.Instance.SaveFile.Save();
	}

	private void OnPressed()
	{
		AppController.Instance.PopupManager.OpenPopupOnTop(new TextPopup.Request("Donation",
			$"Unlock this reward?",
			new TextButton.Parameters("Cancel",
				() =>
				{
				}
			),
			new TextButton.Parameters("Confirm",
				() =>
				{
					GiveRewards().Forget();
				},
				TextButton.ColorType.Green
			)
		));
	}

	private void OnUnlocked(SavedMerchantsGuildHallReward savedMerchantsGuildHallReward)
	{
		UpdateVisuals();
	}
}
using System.Collections.Generic;
using Godot;

public partial class GoldDistributionPopup : Popup<GoldDistributionPopup.Request>
{
	public class Request : PopupRequest
	{
		public int Gold { get; init; }
		public List<SavedCharacter> Characters { get; init; }
	}

	[Export]
	private Label _distributionAmountLabel;

	[Export]
	private PackedScene _characterScene;
	[Export]
	private Control _characterParent;

	[Export]
	private ChoiceButton _confirmButton;

	private readonly List<GoldDistributionPopupCharacter> _characters = new List<GoldDistributionPopupCharacter>();

	public override void _Ready()
	{
		base._Ready();

		_confirmButton.BetterButton.Pressed += OnConfirmPressed;
	}

	protected override void OnOpen()
	{
		base.OnOpen();

		int defaultDistributionAmount = PopupRequest.Gold / PopupRequest.Characters.Count;
		int remainderAmount = PopupRequest.Gold % PopupRequest.Characters.Count;

		for(int i = 0; i < PopupRequest.Characters.Count; i++)
		{
			int distributionAmount = defaultDistributionAmount;
			if(remainderAmount > 0)
			{
				distributionAmount++;
				remainderAmount--;
			}

			SavedCharacter character = PopupRequest.Characters[i];
			GoldDistributionPopupCharacter goldDistributionPopupCharacter = _characterScene.Instantiate<GoldDistributionPopupCharacter>();
			_characterParent.AddChild(goldDistributionPopupCharacter);
			goldDistributionPopupCharacter.Init(character, distributionAmount);
			goldDistributionPopupCharacter.DistributionAmountChangedEvent += OnDistributionAmountChanged;
			_characters.Add(goldDistributionPopupCharacter);
		}

		UpdateVisuals();
	}

	private void UpdateVisuals()
	{
		int remainingAmount = PopupRequest.Gold;
		foreach(GoldDistributionPopupCharacter character in _characters)
		{
			remainingAmount -= character.DistributionAmount;
		}

		foreach(GoldDistributionPopupCharacter character in _characters)
		{
			character.UpdateRemainingGold(remainingAmount);
		}

		_distributionAmountLabel.SetText($"{remainingAmount}/{PopupRequest.Gold}");

		_confirmButton.SetActive(remainingAmount == 0);
	}

	private void OnDistributionAmountChanged(GoldDistributionPopupCharacter character)
	{
		UpdateVisuals();
	}

	private void OnConfirmPressed()
	{
		foreach(GoldDistributionPopupCharacter character in _characters)
		{
			character.SavedCharacter.AddGold(character.DistributionAmount);
		}

		Close();
	}
}
using System;
using Godot;

public partial class GoldDistributionPopupCharacter : Control
{
	[Export]
	private TextureRect _portraitTextureRect;
	[Export]
	private Label _currentGoldLabel;

	[Export]
	private Label _distributionAmountLabel;

	[Export]
	private GoldDistributionPopupButton _increaseButton;
	[Export]
	private GoldDistributionPopupButton _increaseMoreButton;
	[Export]
	private GoldDistributionPopupButton _decreaseButton;
	[Export]
	private GoldDistributionPopupButton _decreaseMoreButton;

	public SavedCharacter SavedCharacter { get; private set; }
	public int DistributionAmount { get; private set; }

	public event Action<GoldDistributionPopupCharacter> DistributionAmountChangedEvent;

	public override void _Ready()
	{
		base._Ready();

		_increaseButton.Button.Pressed += OnIncreasePressed;
		_increaseMoreButton.Button.Pressed += OnIncreaseMorePressed;
		_decreaseButton.Button.Pressed += OnDecreasePressed;
		_decreaseMoreButton.Button.Pressed += OnDecreaseMorePressed;
	}

	public void Init(SavedCharacter savedCharacter, int distributionAmount)
	{
		SavedCharacter = savedCharacter;

		_portraitTextureRect.SetTexture(savedCharacter.ClassModel.PortraitTexture);

		UpdateDistributionAmount(distributionAmount);
	}

	public void UpdateRemainingGold(int remainingAmount)
	{
		_increaseButton.SetActive(remainingAmount >= 1);
		_increaseMoreButton.SetActive(remainingAmount >= 5);
		_decreaseButton.SetActive(DistributionAmount >= 1);
		_decreaseMoreButton.SetActive(DistributionAmount >= 5);
	}

	private void UpdateDistributionAmount(int changeAmount)
	{
		DistributionAmount += changeAmount;
		_distributionAmountLabel.SetText(DistributionAmount.ToString());
		_currentGoldLabel.SetText((SavedCharacter.Gold + DistributionAmount).ToString());

		DistributionAmountChangedEvent?.Invoke(this);
	}

	private void OnIncreasePressed()
	{
		UpdateDistributionAmount(1);
	}

	private void OnIncreaseMorePressed()
	{
		UpdateDistributionAmount(5);
	}

	private void OnDecreasePressed()
	{
		UpdateDistributionAmount(-1);
	}

	private void OnDecreaseMorePressed()
	{
		UpdateDistributionAmount(-5);
	}
}
using Godot;

public partial class BetweenScenariosSidePanel : Control
{
	[Export]
	private ResizingLabel _partyNameLabel;
	[Export]
	private Control _newCharacterButtonContainer;
	[Export]
	private BetterButton _newCharacterButton;

	public override void _Ready()
	{
		base._Ready();

		this.DelayedCall(() =>
		{
			_partyNameLabel.SetText(BetweenScenariosController.Instance.SavedCampaign.PartyName);
		});

		_newCharacterButton.Pressed += OnNewCharacterPressed;

		BetweenScenariosController.Instance.SavedCampaign.CharactersChangedEvent += OnCharactersChanged;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		BetweenScenariosController.Instance.SavedCampaign.CharactersChangedEvent -= OnCharactersChanged;
	}

	private void OnNewCharacterPressed()
	{
		AppController.Instance.PopupManager.RequestPopup(new CreateCharacterPopup.Request()
		{
			SavedCampaign = BetweenScenariosController.Instance.SavedCampaign
		});
	}

	private void OnCharactersChanged()
	{
		_newCharacterButtonContainer.SetVisible(BetweenScenariosController.Instance.SavedCampaign.Characters.Count < 4);
	}
}
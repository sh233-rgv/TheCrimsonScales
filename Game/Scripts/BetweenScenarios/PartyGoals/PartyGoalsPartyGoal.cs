using Godot;

public partial class PartyGoalsPartyGoal : Control
{
	[Export]
	private TextureRect _checkmark;
	[Export]
	private RichTextLabel _label;
	[Export]
	private ProgressBar _progressBar;

	private SavedPartyGoal _savedPartyGoal;

	public void Init(SavedPartyGoal savedPartyGoal)
	{
		_savedPartyGoal = savedPartyGoal;

		_savedPartyGoal.PartyGoalData.ProgressChangedEvent += OnProgressChanged;
		BetweenScenariosController.Instance.SavedCampaign.CharactersChangedEvent += OnCharactersChanged;

		UpdateVisuals();
	}

	public override void _Notification(int what)
	{
		base._Notification(what);

		if(what == NotificationPredelete && AppController.Instance != null)
		{
			if(_savedPartyGoal != null)
			{
				_savedPartyGoal.PartyGoalData.ProgressChangedEvent -= OnProgressChanged;
			}

			if(BetweenScenariosController.Instance != null)
			{
				BetweenScenariosController.Instance.SavedCampaign.CharactersChangedEvent -= OnCharactersChanged;
			}
		}
	}

	private void UpdateVisuals()
	{
		_label.SetText(_savedPartyGoal.Model.GetText(BetweenScenariosController.Instance.SavedCampaign.Characters.Count));

		if(_savedPartyGoal.Model.ScalesWithCharacterCount)
		{
			//TODO
		}
	}

	private void OnProgressChanged(PartyGoalData partyGoalData)
	{
		UpdateVisuals();
	}

	private void OnCharactersChanged()
	{
		UpdateVisuals();
	}
}
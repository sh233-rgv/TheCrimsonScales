using Godot;

public partial class PortraitViewCharacterPortrait : PortraitViewPortrait
{
	[Export]
	private ExclamationMark _exclamationMark;

	public Character Character { get; private set; }

	public override Initiative Initiative => Character.Initiative;

	public void Init(Character character)
	{
		base.Init();

		Character = character;

		_portraitTexture.Texture = Character.PortraitTexture;

		Character.InitiativeChangedEvent += OnInitiativeChanged;
		Character.BattleGoalChangedEvent += OnBattleGoalChangedEvent;

		OnInitiativeChanged(Character);

		UpdateExclamationMark();
	}

	public override void Destroy()
	{
		base.Destroy();

		if(Character != null)
		{
			Character.InitiativeChangedEvent -= OnInitiativeChanged;
			Character.BattleGoalChangedEvent -= OnBattleGoalChangedEvent;
		}
	}

	protected override void OnTurnTakerChanged(Figure figure)
	{
		base.OnTurnTakerChanged(figure);

		SetSelected(figure == Character);
	}

	private void OnInitiativeChanged(Figure figure)
	{
		_initiativeLabel.Text = figure.Initiative.ToString();

		GameController.Instance.PortraitView.Reorder();

		UpdateExclamationMark();
	}

	private void OnBattleGoalChangedEvent(Character character)
	{
		UpdateExclamationMark();
	}

	private void UpdateExclamationMark()
	{
		_exclamationMark.SetActive(Character.SelectedBattleGoalModel == null);
	}
}
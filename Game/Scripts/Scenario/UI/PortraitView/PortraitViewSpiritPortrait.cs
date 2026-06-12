public partial class PortraitViewSpiritPortrait : PortraitViewPortrait
{
	public Spirit Spirit { get; private set; }

	public override Initiative Initiative => Spirit.Initiative;

	public void Init(Spirit spirit)
	{
		base.Init();

		Spirit = spirit;

		_portraitTexture.SetTexture(Spirit.PortraitTexture);

		Spirit.InitiativeChangedEvent += OnInitiativeChanged;

		OnInitiativeChanged(Spirit);
	}

	public override void Destroy()
	{
		base.Destroy();

		if(Spirit != null)
		{
			Spirit.InitiativeChangedEvent -= OnInitiativeChanged;
		}
	}

	protected override void OnTurnTakerChanged(Figure figure)
	{
		base.OnTurnTakerChanged(figure);

		SetSelected(figure == Spirit);
	}

	private void OnInitiativeChanged(Figure figure)
	{
		_initiativeLabel.SetText(figure.Initiative.ToString());

		GameController.Instance.PortraitView.Reorder();
	}
}
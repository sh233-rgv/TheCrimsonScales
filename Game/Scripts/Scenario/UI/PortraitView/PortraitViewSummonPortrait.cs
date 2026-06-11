public partial class PortraitViewSummonPortrait : PortraitViewPortrait
{
	public Summon Summon { get; private set; }

	public override Initiative Initiative => Summon.Initiative;

	public void Init(Summon summon)
	{
		base.Init();

		Summon = summon;

		_portraitTexture.Texture = Summon.Texture;

		Summon.InitiativeChangedEvent += OnInitiativeChanged;

		OnInitiativeChanged(Summon);
	}

	public override void Destroy()
	{
		base.Destroy();

		if(Summon != null)
		{
			Summon.InitiativeChangedEvent -= OnInitiativeChanged;
		}
	}

	protected override void OnTurnTakerChanged(Figure figure)
	{
		base.OnTurnTakerChanged(figure);

		SetSelected(figure == Summon);
	}

	private void OnInitiativeChanged(Figure figure)
	{
		_initiativeLabel.Text = figure.Initiative.ToString();

		GameController.Instance.PortraitView.Reorder();
	}
}
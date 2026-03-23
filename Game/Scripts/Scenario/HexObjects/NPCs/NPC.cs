using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public partial class NPC : Figure
{
	private NPCViewComponent _npcViewComponent;
	private string _name;
	private readonly List<Ability> _abilities = new List<Ability>();
	private AMDCardDeck _amdCardDeckOverride;

	private ActionState _turnActionState;


	public string AssetPath { get; private set; }
	public List<Ability> Abilities => _abilities;
	public Initiative PermanentInitiative { get; private set; }

	public override string DisplayName => _name;
	public override string DebugName => _name;
	public override AMDCardDeck AMDCardDeck => _amdCardDeckOverride ?? GameController.Instance.MonsterAMDCardDeck;
	public virtual Texture2D Texture => ResourceLoader.Load<Texture2D>($"{AssetPath}/Artwork.jpg");
	public virtual Texture2D PortraitTexture => ResourceLoader.Load<Texture2D>($"{AssetPath}/Portrait.tres");
	public override Texture2D MapIconTexture => ResourceLoader.Load<Texture2D>($"{AssetPath}/MapIcon.tres");
	public override Node2D Visual => _npcViewComponent.Sprite;

	public override async GDTask Init(Hex originHex, int rotationIndex = 0, bool hexCanBeNull = false)
	{
		await base.Init(originHex, rotationIndex, hexCanBeNull);

		_npcViewComponent = GetViewComponent<NPCViewComponent>();
	}

	public void Spawn(int health, string name, string assetPath, List<Ability> abilities, int initiative, Alignment alignment, Alignment enemies)
	{
		_name = $"{name} - NPC";
		PermanentInitiative = new Initiative()
		{
			MainInitiative = initiative,
			SortingInitiative = initiative * 10000000 + 8000000
		};
		AssetPath = assetPath;

		_outline.SetSelfModulate(Color.FromHtml("1778ff"));
		_figureViewComponent.TurnStartPS.SetSelfModulate(Color.FromHtml("1778ff"));
		_figureViewComponent.ActivePS.SetModulate(Color.FromHtml("1778ff"));

		_npcViewComponent.Sprite.SetTexture(MapIconTexture);
		float textureWidth = MapIconTexture.GetWidth();
		_npcViewComponent.Sprite.SetScale(250f / textureWidth * Vector2.One);

		SetMaxHealth(health);
		SetHealth(health);

		SetAlignment(alignment);
		SetEnemies(enemies);

		GameController.Instance.Map.RegisterFigure(this);

		UpdateInitiative();

		_abilities.AddRange(abilities);
	}

	protected override async GDTask TakeTurn()
	{
		await base.TakeTurn();

		_turnActionState = new ActionState(this, this, _abilities);
		await _turnActionState.Perform();
	}

	public async GDTask RemoveTurnActionFromActive()
	{
		if(_turnActionState != null)
		{
			await _turnActionState.RemoveFromActive();
		}
	}

	public override async GDTask Destroy(bool immediately = false, bool forceDestroy = false)
	{
		await RemoveTurnActionFromActive();

		await base.Destroy(immediately, forceDestroy);
	}

	protected override Initiative GetInitiative()
	{
		return PermanentInitiative;
	}

	public override void AddInfoItemParameters(List<InfoItemParameters> parametersList)
	{
		base.AddInfoItemParameters(parametersList);

		parametersList.Add(new NPCInfoItem.Parameters(this));
	}

	public void SetAMDCardDeck(AMDCardDeck amdCardDeck)
	{
		_amdCardDeckOverride = amdCardDeck;
	}
}
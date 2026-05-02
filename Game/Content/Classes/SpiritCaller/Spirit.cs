using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public partial class Spirit : Figure
{
	private SpiritViewComponent _spiritViewComponent;
	private string _name;
	private readonly List<Ability> _abilities = new List<Ability>();

	private ActionState _turnActionState;

	public int HealthStat { get; private set; }
	public int? Move { get; private set; }
	public int? Attack { get; private set; }
	public int? Range { get; private set; }
	public Character CharacterOwner { get; private set; }
	public Texture2D Texture { get; private set; }
	public int SpiritIndex { get; private set; }

	public override string DisplayName => _name;
	public override string DebugName => _name;
	public override AMDCardDeck AMDCardDeck => CharacterOwner.AMDCardDeck;
	public override Texture2D MapIconTexture => _spiritViewComponent.Sprite.Texture;
	public override Node2D Visual => _spiritViewComponent.Sprite;

	public override bool IsFigure => false;

	public RangeType RangeType => Range.HasValue ? RangeType.Range : RangeType.Melee;

	public override async GDTask Init(Hex originHex, int rotationIndex = 0, bool hexCanBeNull = false)
	{
		await base.Init(originHex, rotationIndex, hexCanBeNull);

		_spiritViewComponent = GetViewComponent<SpiritViewComponent>();
	}

	public async GDTask Spawn(int health, int? move, int? attack, int? range, FigureTrait[] traits, Character characterOwner, string name,
		string texturePath, string mapIconTexturePath)
	{
		HealthStat = health;
		Move = move;
		Attack = attack;
		Range = range;

		CharacterOwner = characterOwner;
		_name = name;

		_outline.SetSelfModulate(CharacterOwner.OutlineColor);
		_figureViewComponent.TurnStartPS.SetSelfModulate(OutlineColor);
		_figureViewComponent.ActivePS.SetModulate(OutlineColor);

		_spiritViewComponent.StandeeNumberCircle.SetSelfModulate(OutlineColor);

		Texture = ResourceLoader.Load<Texture2D>(texturePath);
		Texture2D mapIconTexture = ResourceLoader.Load<Texture2D>(mapIconTexturePath);
		_spiritViewComponent.Sprite.SetTexture(mapIconTexture);
		float textureWidth = mapIconTexture.GetWidth();
		_spiritViewComponent.Sprite.SetScale((250f / textureWidth) * Vector2.One);

		SetMaxHealth(HealthStat);
		SetHealth(HealthStat);

		SetAlignment(CharacterOwner.Alignment);
		SetEnemies(CharacterOwner.Enemies);

		if(traits != null)
		{
			foreach(FigureTrait trait in traits)
			{
				await AddTrait(trait);
			}
		}

		RegisterSpirit(this);

		await GameController.Instance.Map.RegisterFigure(this);

		UpdateInitiative();

		if(Move.HasValue)
		{
			MoveAbility moveAbility = MoveAbility.Builder().WithDistance(Move.Value).Build();
			_abilities.Add(moveAbility);
		}

		if(Attack.HasValue)
		{
			AttackAbility attackAbility = AttackAbility.Builder()
				.WithDamage(Attack.Value)
				.WithRange(Range ?? 1)
				.WithRangeType(RangeType)
				.Build();
			_abilities.Add(attackAbility);
		}

		ScenarioEvents.HexObjectDestroyedEvent.Subscribe(this, CharacterOwner,
			parameters => parameters.HexObject == CharacterOwner,
			async parameters =>
			{
				await Destroy(parameters.Immediately, parameters.ForceDestroy);
			}
		);
	}

	protected override async GDTask TakeTurn()
	{
		await base.TakeTurn();

		Figure authority = CharacterOwner;

		_turnActionState = new ActionState(this, this, authority, _abilities);
		await _turnActionState.Perform();
	}

	protected override async GDTask EndTurn()
	{
		await base.EndTurn();

		// Spirits suffer 1 damage at the end of their turns
		await AbilityCmd.SufferDamage(this, 1, this);
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

		ScenarioEvents.HexObjectDestroyedEvent.Unsubscribe(this, CharacterOwner);

		DeregisterSpirit(this);

		await base.Destroy(immediately, forceDestroy);
	}

	protected override Initiative GetInitiative()
	{
		Initiative ownerInitiative = CharacterOwner.Initiative;
		if(ownerInitiative.Null)
		{
			return new Initiative()
			{
				Null = true
			};
		}

		return new Initiative()
		{
			MainInitiative = ownerInitiative.MainInitiative,
			SortingInitiative = ownerInitiative.SortingInitiative + SpiritIndex
		};
	}

	private void SetSpiritIndex(int spiritIndex)
	{
		SpiritIndex = spiritIndex;

		UpdateInitiative();

		_spiritViewComponent.StandeeNumberLabel.SetText((SpiritIndex + 1).ToString());
	}

	private void RegisterSpirit(Spirit spirit)
	{
		List<Spirit> spirits = GetSpirits(spirit.CharacterOwner);

		spirits.Add(this);
		SetSpiritIndex(spirits.Count - 1);
	}

	private void DeregisterSpirit(Spirit spirit)
	{
		List<Spirit> spirits = GetSpirits(spirit.CharacterOwner);

		for(int i = 0; i < spirits.Count; i++)
		{
			Spirit otherSpirit = spirits[i];
			otherSpirit.SetSpiritIndex(i);
		}
	}

	public static List<Spirit> GetSpirits(Character characterOwner)
	{
		const string spiritsKey = "Spirits";
		if(!characterOwner.TryGetCustomValue(spiritsKey, out List<Spirit> spirits))
		{
			spirits = new List<Spirit>();
			characterOwner.SetCustomValue(spiritsKey, spirits);
		}

		return spirits;
	}

	public override void AddInfoItemParameters(List<InfoItemParameters> parametersList)
	{
		base.AddInfoItemParameters(parametersList);

		parametersList.Add(new SpiritInfoItem.Parameters(this));
	}
}
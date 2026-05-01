using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public partial class Spirit : Figure
{
	private SpiritViewComponent _summonViewComponent;
	private string _name;
	private readonly List<Ability> _abilities = new List<Ability>();

	private ActionState _turnActionState;

	public int Health { get; private set; }
	public int? Move { get; private set; }
	public int? Attack { get; private set; }
	public int? Range { get; private set; }
	public Character CharacterOwner { get; private set; }
	public Texture2D Texture { get; private set; }
	public int SpiritIndex { get; private set; }

	public override string DisplayName => _name;
	public override string DebugName => _name;
	public override AMDCardDeck AMDCardDeck => CharacterOwner.AMDCardDeck;
	public override Texture2D MapIconTexture => _summonViewComponent.Sprite.Texture;
	public override Node2D Visual => _summonViewComponent.Sprite;

	public RangeType RangeType => Range.HasValue ? RangeType.Range : RangeType.Melee;

	public override async GDTask Init(Hex originHex, int rotationIndex = 0, bool hexCanBeNull = false)
	{
		await base.Init(originHex, rotationIndex, hexCanBeNull);

		_summonViewComponent = GetViewComponent<SpiritViewComponent>();
	}

	public async GDTask Spawn(int health, int? move, int? attack, int? range, FigureTrait[] traits, Character characterOwner, string name,
		string texturePath, string mapIconTexturePath)
	{
		Health = health;
		Move = move;
		Attack = attack;
		Range = range;

		CharacterOwner = characterOwner;
		_name = name;

		_outline.SetSelfModulate(CharacterOwner.OutlineColor);
		_figureViewComponent.TurnStartPS.SetSelfModulate(OutlineColor);
		_figureViewComponent.ActivePS.SetModulate(OutlineColor);

		_summonViewComponent.StandeeNumberCircle.SetSelfModulate(OutlineColor);

		Texture = ResourceLoader.Load<Texture2D>(texturePath);
		Texture2D mapIconTexture = ResourceLoader.Load<Texture2D>(mapIconTexturePath);
		_summonViewComponent.Sprite.SetTexture(mapIconTexture);
		float textureWidth = mapIconTexture.GetWidth();
		_summonViewComponent.Sprite.SetScale((250f / textureWidth) * Vector2.One);

		SetMaxHealth(Health);
		SetHealth(Health);

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

		CanTakeTurn = false;

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

		// ScenarioEvents.FigureFoundFocusEvent.Subscribe(this, CharacterOwner,
		// 	parameters =>
		// 		parameters.Performer == this &&
		// 		parameters.AbilityState is MoveAbility.State &&
		// 		parameters.Focus == null,
		// 	async parameters =>
		// 	{
		// 		parameters.SetNewFocus(CharacterOwner);
		//
		// 		ScenarioCheckEvents.AIMoveParametersCheckEvent.Subscribe(this, CharacterOwner,
		// 			parameters => parameters.Performer == this,
		// 			parameters =>
		// 			{
		// 				parameters.SetRange(1);
		// 				parameters.SetRangeType(RangeType.Melee);
		// 				parameters.SetTargets(1);
		// 				parameters.SetAOEPattern(null);
		// 			}
		// 		);
		//
		// 		ScenarioEvents.AbilityEndedEvent.Subscribe(this, CharacterOwner,
		// 			parameters => parameters.Performer == this,
		// 			async parameters =>
		// 			{
		// 				ScenarioEvents.AbilityEndedEvent.Unsubscribe(this, CharacterOwner);
		// 				ScenarioCheckEvents.AIMoveParametersCheckEvent.Unsubscribe(this, CharacterOwner);
		//
		// 				await GDTask.CompletedTask;
		// 			}
		// 		);
		//
		// 		await GDTask.CompletedTask;
		// 	},
		// 	effectType: EffectType.Selectable,
		// 	effectButtonParameters: new IconEffectButton.Parameters(Icons.Move),
		// 	effectInfoViewParameters: new TextEffectInfoView.Parameters("Choose for the summon to move towards the summoner")
		// );
	}

	protected override async GDTask TakeTurn()
	{
		await base.TakeTurn();

		Figure authority = CharacterOwner;

		_turnActionState = new ActionState(this, this, authority, _abilities);
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

		//ScenarioEvents.FigureFoundFocusEvent.Unsubscribe(this, CharacterOwner);
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
			SortingInitiative = ownerInitiative.SortingInitiative - 100 + SpiritIndex
		};
	}

	private void SetSpiritIndex(int spiritIndex)
	{
		SpiritIndex = spiritIndex;

		UpdateInitiative();

		_summonViewComponent.StandeeNumberLabel.SetText((SpiritIndex + 1).ToString());
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

	private List<Spirit> GetSpirits(Character characterOwner)
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
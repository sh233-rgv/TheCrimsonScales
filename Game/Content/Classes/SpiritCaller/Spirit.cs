using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class Spirit : Figure
{
	[Export]
	private Node2D _container;
	[Export]
	private Node2D _mask;

	private SpiritViewComponent _spiritViewComponent;
	private string _name;
	private readonly List<Ability> _abilities = new List<Ability>();

	private ActionState _turnActionState;

	private bool _inCorner;

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

	public override void _Ready()
	{
		base._Ready();

		_spiritViewComponent = GetViewComponent<SpiritViewComponent>();

		_mask.Reparent(_container, false);
		_outline.Reparent(_container, false);
		FigureViewComponent.Reparent(_container, false);
		_spiritViewComponent.Reparent(_container, false);
	}

	public override async GDTask Init(Hex originHex, int rotationIndex = 0, bool hexCanBeNull = false)
	{
		await base.Init(originHex, rotationIndex, hexCanBeNull);
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
		FigureViewComponent.TurnStartPS.SetSelfModulate(OutlineColor);
		FigureViewComponent.ActivePS.SetModulate(OutlineColor);

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
			parameters =>
				parameters.HexObject == CharacterOwner ||
				parameters.HexObject.Hex == Hex,
			async parameters =>
			{
				if(parameters.HexObject == CharacterOwner)
				{
					await Destroy(parameters.Immediately, parameters.ForceDestroy);
				}
				else
				{
					await UpdateInCorner(0f);
				}
			}
		);

		ScenarioEvents.FigureEnteredHexEvent.Subscribe(this, CharacterOwner,
			parameters =>
				parameters.Hex == Hex ||
				parameters.Figure == this,
			async parameters =>
			{
				await UpdateInCorner();
			}
		);

		ScenarioCheckEvents.FlyingCheckEvent.Subscribe(this, CharacterOwner,
			parameters => parameters.Figure == this,
			parameters => parameters.SetFlying(true)
		);

		ScenarioCheckEvents.CanBeFocusedCheckEvent.Subscribe(this, CharacterOwner,
			canApplyParameters =>
				canApplyParameters.PotentialTarget == this,
			applyParameters =>
			{
				applyParameters.SetCannotBeFocused();
			}
		);

		ScenarioCheckEvents.CanBeTargetedCheckEvent.Subscribe(this, CharacterOwner,
			canApplyParameters =>
				canApplyParameters.PotentialTarget == this,
			applyParameters =>
			{
				applyParameters.SetCannotBeTargeted();
			}
		);

		// Allow stopping movement in the same hex
		ScenarioCheckEvents.CanStopMoveAtHexWithFigureCheckEvent.Subscribe(this, CharacterOwner,
			parameters =>
				parameters.OtherFigure == this &&
				parameters.Figure is not Spirit,
			parameters =>
			{
				parameters.SetCanStopAt();
			}
		);

		ScenarioCheckEvents.CanPassEnemyCheckEvent.Subscribe(this, CharacterOwner,
			parameters =>
				parameters.EnemyFigure == this,
			parameters =>
			{
				parameters.SetCanPass();
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
		ScenarioEvents.FigureEnteredHexEvent.Unsubscribe(this, CharacterOwner);
		ScenarioCheckEvents.FlyingCheckEvent.Unsubscribe(this, CharacterOwner);
		ScenarioCheckEvents.CanBeFocusedCheckEvent.Unsubscribe(this, CharacterOwner);
		ScenarioCheckEvents.CanBeTargetedCheckEvent.Unsubscribe(this, CharacterOwner);
		ScenarioCheckEvents.CanStopMoveAtHexWithFigureCheckEvent.Unsubscribe(this, CharacterOwner);
		ScenarioCheckEvents.CanPassEnemyCheckEvent.Unsubscribe(this, CharacterOwner);

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

	private async GDTask UpdateInCorner(float initialDelay = 0f)
	{
		bool inCorner = Hex.GetFigures().Any();
		if(inCorner != _inCorner)
		{
			if(inCorner)
			{
				FigureViewComponent.SetCanAdjustViewPosition(false);
				await GTweenSequenceBuilder.New()
					.AppendTime(initialDelay)
					.Append(_container.TweenScale(0.7f, 0.3f).SetEasing(Easing.InOutBack))
					.Join(_container.TweenPosition(new Vector2(-70f, -70f), 0.3f).SetEasing(Easing.OutBack))
					.Build().PlayFastForwardableAsync();
			}
			else
			{
				FigureViewComponent.SetCanAdjustViewPosition(true);
				_container.TweenScale(1f, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardable();
				_container.TweenPosition(Vector2.Zero, 0.3f).SetEasing(Easing.OutBack).PlayFastForwardable();
				await GDTask.DelayFastForwardable(0.3f);
			}

			_inCorner = inCorner;
		}
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
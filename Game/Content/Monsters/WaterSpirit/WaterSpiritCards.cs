using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public abstract class WaterSpiritAbilityCard : MonsterAbilityCardModel
{
	public override string CardsAtlasPath => "res://Content/Monsters/WaterSpirit/Cards.jpg";

	public static IEnumerable<MonsterAbilityCardModel> Deck { get; } =
	[
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard0>(),
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard1>(),
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard2>(),
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard3>(),
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard4>(),
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard5>(),
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard6>(),
		ModelDB.MonsterAbilityCard<WaterSpiritAbilityCard7>()
	];

	public async GDTask TryCreateWaterTile(Hex hex)
	{
		if(hex.IsFeatureless())
		{
			await AbilityCmd.CreateDifficultTerrain(hex,
				ResourceLoader.Load<PackedScene>("res://Content/OverlayTiles/DifficultTerrain/Water1H.tscn"));
		}
	}
}

public class WaterSpiritAbilityCard0 : WaterSpiritAbilityCard
{
	public override int Initiative => 37;
	public override int CardIndex => 0;
	public override bool Reshuffles => true;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0).WithAfterTargetConfirmedSubscription(
			ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
				applyFunction: async applyParameters =>
				{
					if(applyParameters.AbilityState.Target.Hex.HasHexObjectOfType<Water>())
					{
						applyParameters.AbilityState.SingleTargetAdjustAttackValue(2);
					}

					await GDTask.CompletedTask;
				}
			)))
	];
}

public class WaterSpiritAbilityCard1 : WaterSpiritAbilityCard
{
	public override int Initiative => 30;
	public override int CardIndex => 1;
	public override bool Reshuffles => true;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, +0).WithPierce(new DynamicInt<AttackAbility.State>(state =>
			{
				List<Hex> hexes = new List<Hex>();
				RangeHelper.FindHexesInRange(state.Performer.Hex, 1, false, hexes);
				int waterHexCount = hexes.Count(hex => hex.HasHexObjectOfType<Water>());
				return waterHexCount;
			})
		)),
	];
}

public class WaterSpiritAbilityCard2 : WaterSpiritAbilityCard
{
	public override int Initiative => 16;
	public override int CardIndex => 2;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, -1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, -1)),
		new MonsterAbilityCardAbility(SufferDamageAbility.Builder()
			.WithDamage(1)
			.WithCustomGetTargets((state, list) =>
				{
					foreach(Figure figure in GameController.Instance.Map.Figures)
					{
						if(state.Authority.EnemiesWith(figure) && figure.Hex.HasHexObjectOfType<Water>())
						{
							list.Add(figure);
						}
					}
				}
			)
		),
	];
}

public class WaterSpiritAbilityCard3 : WaterSpiritAbilityCard
{
	public override int Initiative => 69;
	public override int CardIndex => 3;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +1)),
		new MonsterAbilityCardAbility(AttackAbility(monster, -1)
			.WithAOEPattern(new AOEPattern(
			[
				new AOEHex(Vector2I.Zero, AOEHexType.Gray),
				new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
				new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red)
			]))
			.WithAfterAttackPerformedSubscription(
				ScenarioEvents.AfterAttackPerformed.Subscription.New(
					applyFunction: async applyParameters =>
					{
						await TryCreateWaterTile(applyParameters.AbilityState.Target.Hex);
					}
				)
			)
		)
	];
}

public class WaterSpiritAbilityCard4 : WaterSpiritAbilityCard
{
	public override int Initiative => 44;
	public override int CardIndex => 4;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(AttackAbility(monster, +0).WithRange(3).WithPull(2).WithConditions(Conditions.Immobilize))
	];
}

public class WaterSpiritAbilityCard5 : WaterSpiritAbilityCard
{
	public override int Initiative => 53;
	public override int CardIndex => 5;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(AttackAbility(monster, new DynamicInt<AttackAbility.State>(-1, state =>
			{
				List<Hex> hexes = new List<Hex>();
				RangeHelper.FindHexesInRange(state.Performer.Hex, 1, false, hexes);
				int waterHexCount = Mathf.Min(hexes.Count(hex => hex.HasHexObjectOfType<Water>()), 3);
				return waterHexCount;
			}
		))),
	];
}

public class WaterSpiritAbilityCard6 : WaterSpiritAbilityCard
{
	public override int Initiative => 86;
	public override int CardIndex => 6;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MoveAbility(monster, +2)),
		new MonsterAbilityCardAbility(HealAbility.Builder().WithHealValue(3).WithTarget(Target.Self)),
		new MonsterAbilityCardAbility(OtherAbility.Builder()
			.WithPerformAbility(async state =>
				{
					Hex hex = await AbilityCmd.SelectHex(state, list =>
					{
						int closestRange = int.MaxValue;
						foreach(Hex neighbourHex in state.Performer.Hex.Neighbours)
						{
							if(!neighbourHex.IsFeatureless())
							{
								continue;
							}

							foreach(Figure figure in GameController.Instance.Map.Figures)
							{
								if(state.Performer.EnemiesWith(figure))
								{
									int range = RangeHelper.Distance(neighbourHex, figure.Hex);
									if(range == closestRange)
									{
										list.Add(neighbourHex);
									}
									else if(range < closestRange)
									{
										closestRange = range;
										list.Clear();
										list.Add(neighbourHex);
									}
								}
							}
						}
					}, mandatory: true);

					if(hex != null)
					{
						await TryCreateWaterTile(hex);
					}
				}
			)
		)
	];
}

public class WaterSpiritAbilityCard7 : WaterSpiritAbilityCard
{
	public override int Initiative => 22;
	public override int CardIndex => 7;

	public override IEnumerable<MonsterAbilityCardAbility> GetAbilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(AttackAbility(monster, +1).WithTargets(2).WithAfterTargetConfirmedSubscription(
			ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
				applyFunction: async applyParameters =>
				{
					await TryCreateWaterTile(applyParameters.AbilityState.Target.Hex);
				}
			))),
	];
}
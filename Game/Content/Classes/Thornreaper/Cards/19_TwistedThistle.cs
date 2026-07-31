using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class TwistedThistle : ThornreaperCardModel<TwistedThistle.CardTop, TwistedThistle.CardBottom>
{
	public override string Name => "Twisted Thistle";
	public override int Level => 4;
	public override int Initiative => 26;
	protected override int AtlasIndex => 19;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.New(
						_ => LightStrongOrWaning,
						async parameters =>
						{
							parameters.AbilityState.AbilityAdjustPush(1);

							await GDTask.CompletedTask;
						}))
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red),
						]
					),
					new AOEHexMark(Vector2I.Zero.Add(Direction.East), this, new Vector2(0.7636514f, 0.21440443f)))
				.Build()),
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithCount(2)
				.WithCustomSelectHexes((state, hexes) =>
				{
					hexes.AddRange(state.ActionState.GetAbilityState<AttackAbility.State>(0).GetRedAOEHexes().Where(hex => hex.IsFeatureless()));
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build()),
		];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4, new MoveCircle(this, new Vector2(0.6185266f, 0.61828256f)))
				.WithOnAbilityStarted(async abilityState =>
				{
					ScenarioCheckEvents.MoveCheckEvent.Subscribe(abilityState, this,
						canApplyParameters => canApplyParameters.AbilityState == abilityState &&
						                      canApplyParameters.Hex.HasHexObjectOfType<HazardousTerrain>(),
						applyParameters =>
						{
							applyParameters.SetAffectedByNegativeHex(false);
						}
					);

					ScenarioEvents.HazardousTerrainTriggeredEvent.Subscribe(abilityState, this,
						canApplyParameters => canApplyParameters.PotentialAbilityState?.Performer == abilityState.Performer,
						async applyParameters =>
						{
							applyParameters.SetAffectedByHazardousTerrain(false);
							await GDTask.CompletedTask;
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnAbilityEnded(async abilityState =>
					{
						ScenarioCheckEvents.MoveCheckEvent.Unsubscribe(abilityState, this);
						ScenarioEvents.HazardousTerrainTriggeredEvent.Unsubscribe(abilityState, this);

						await GDTask.CompletedTask;
					}
				)
				.WithOnAbilityEndedPerformed(async state =>
				{
					List<HazardousTerrain> hazardousTerrainTiles = [];
					foreach(Hex hex in state.Hexes)
					{
						if(hex.TryGetHexObjectOfType(out HazardousTerrain hazardousTerrain) && !hazardousTerrain.CannotBeDestroyed)
						{
							hazardousTerrainTiles.Add(hazardousTerrain);
						}
					}

					List<Hex> hexes =
				})
				.Build())
		];
	}
}
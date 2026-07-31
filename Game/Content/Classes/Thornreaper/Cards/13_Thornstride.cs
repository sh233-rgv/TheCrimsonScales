using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class Thornstride : ThornreaperCardModel<Thornstride.CardTop, Thornstride.CardBottom>
{
	public override string Name => "Thornstride";
	public override int Level => 1;
	public override int Initiative => 24;
	protected override int AtlasIndex => 13;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Earth)),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.61930263f, 0.14958449f)))
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.East), AOEHexType.Red),
						]
					),
					new AOEHexMark(Vector2I.Zero.Add(Direction.East).Add(Direction.East).Add(Direction.East), this,
						new Vector2(0.7000138f, 0.4167205f)),
					new AOEHexMark(Vector2I.Zero.Add(Direction.West), this, new Vector2(0.3003385f, 0.41667458f)))
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.New(
						parameters => parameters.Performer.Hex.HasHexObjectOfType<HazardousTerrain>(),
						async parameters =>
						{
							parameters.AbilityState.AbilityAdjustPierce(1);

							await AbilityCmd.GainXP(parameters.Performer, 1);
						}, canApplyMultipleTimesDuringSubscription: false))
				.WithAfterTargetConfirmedSubscription(
					ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
						parameters => parameters.AbilityState.Target.Hex.HasHexObjectOfType<HazardousTerrain>(),
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAdjustAttackValue(1);

							await GDTask.CompletedTask;
						}))
				.Build())
		];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
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
				.Build())
		];
	}
}
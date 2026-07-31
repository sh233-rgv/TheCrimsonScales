using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ExtendedBranch : ThornreaperCardModel<ExtendedBranch.CardTop, ExtendedBranch.CardBottom>
{
	public override string Name => "Extended Branch";
	public override int Level => 1;
	public override int Initiative => 61;
	protected override int AtlasIndex => 3;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackSquare(this, new Vector2(0.61930263f, 0.14521438f)))
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.NorthEast), AOEHexType.Red),
						]
					), new AOEHexMark(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.NorthWest), this, new Vector2(0.43412223f, 0.30415514f)),
					new AOEHexMark(Vector2I.Zero.Add(Direction.East).Add(Direction.NorthEast).Add(Direction.NorthEast), this,
						new Vector2(0.634048f, 0.30470914f)))
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.New(
						_ => LightStrongOrWaning,
						async parameters =>
						{
							parameters.AbilityState.AbilityAdjustAttackValue(1);

							await AbilityCmd.InfuseElement(parameters.AbilityState, Element.Light);
							await AbilityCmd.GainXP(parameters.Performer, 1);
						}, canApplyMultipleTimesDuringSubscription: false))
				.Build())
		];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioCheckEvents.MoveCheckEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Performer.AlliedWith(state.Performer) &&
						                      canApplyParameters.Hex.HasHexObjectOfType<ThornsThornreaper>(),
						applyParameters =>
						{
							applyParameters.SetAffectedByNegativeHex(false);
						}
					);

					ScenarioEvents.HazardousTerrainTriggeredEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Figure.AlliedWith(state.Performer) &&
						                      canApplyParameters.HazardousTerrain is ThornsThornreaper,
						async applyParameters =>
						{
							applyParameters.SetAffectedByHazardousTerrain(false);
							await GDTask.CompletedTask;
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioCheckEvents.MoveCheckEvent.Unsubscribe(state, this);
					ScenarioEvents.HazardousTerrainTriggeredEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 1;
		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
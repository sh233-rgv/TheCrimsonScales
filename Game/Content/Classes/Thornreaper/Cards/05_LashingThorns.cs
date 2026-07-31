using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class LashingThorns : ThornreaperCardModel<LashingThorns.CardTop, LashingThorns.CardBottom>
{
	public override string Name => "Lashing Thorns";
	public override int Level => 1;
	public override int Initiative => 39;
	protected override int AtlasIndex => 5;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Earth)),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithPierce(1, new PierceSquare(this, new Vector2(0.53626335f, 0.27293023f)))
				.WithAOEPattern(new AOEPattern(
				[
					new AOEHex(Vector2I.Zero, AOEHexType.Gray),
					new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red),
				]))
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.New(
						parameters => parameters.Performer.Hex.HasHexObjectOfType<HazardousTerrain>(),
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
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4, new MoveCircle(this, new Vector2(0.5226701f, 0.6786704f)))
				.WithMoveType(MoveType.Jump)
				.Build()),
			new AbilityCardAbility(PullAbility.Builder()
				.WithPull(2)
				.WithTargets(2, new TargetsSquare(this, new Vector2(0.5122052f, 0.77265316f)))
				.WithRange(3, new RangeSquare(this, new Vector2(0.72311985f, 0.77239925f)))
				.Build())
		];

		public override int XP => 1;
		public override bool Loss => true;
	}
}
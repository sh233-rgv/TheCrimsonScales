using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class RavagedEarth : ThornreaperCardModel<RavagedEarth.CardTop, RavagedEarth.CardBottom>
{
	public override string Name => "Ravaged Earth";
	public override int Level => 1;
	public override int Initiative => 86;
	protected override int AtlasIndex => 28;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(5, new AttackSquare(this, new Vector2(0.50932485f, 0.17174517f)))
				.WithConditions(Conditions.Muddle)
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthWest).Add(Direction.NorthWest), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.SouthEast), AOEHexType.Red),
						]))
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(5, new MoveCircle(this, new Vector2(0.52307016f, 0.68456453f)))
				.WithMoveType(MoveType.Jump)
				.Build()),
			new AbilityCardAbility(SufferDamageAbility.Builder()
				.WithDamage(1)
				.WithRange(1)
				.WithTarget(Target.Enemies | Target.TargetAll)
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Earth)];
	}
}
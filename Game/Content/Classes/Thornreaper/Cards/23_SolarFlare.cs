using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;
using Range = Godot.Range;

public class SolarFlare : ThornreaperCardModel<SolarFlare.CardTop, SolarFlare.CardBottom>
{
	public override string Name => "Solar Flare";
	public override int Level => 6;
	public override int Initiative => 36;
	protected override int AtlasIndex => 23;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.44934365f, 0.16620499f)))
				.WithRange(4, new RangeSquare(this, new Vector2(0.65995824f, 0.16565098f)))
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red)
					]
				), new AOEHexMark(Vector2I.Zero.Add(Direction.NorthWest), this, new Vector2(0.39977527f, 0.25183988f)))
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements =>
			[CardElementInfusion.Infuse(Element.Fire), CardElementInfusion.Infuse(Element.Light)];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Earth, Element.Light)),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.62115484f, 0.80796415f)))
				.Build())
		];
	}
}
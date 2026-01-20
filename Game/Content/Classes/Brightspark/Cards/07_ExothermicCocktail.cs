using System.Collections.Generic;
using Godot;

public class ExothermicCocktail : BrightsparkCardModel<ExothermicCocktail.CardTop, ExothermicCocktail.CardBottom>
{
	public override string Name => "Exothermic Cocktail";
	public override int Level => 1;
	public override int Initiative => 45;
	protected override int AtlasIndex => 7;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Empty),
						new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.East), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.SouthEast), AOEHexType.Red),
					]
				))
				.Build()),
		];

		public override IEnumerable<Element> Elements => [Element.Fire];
		public override int XP => 1;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
				.Build())
		];
	}
}
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class Midsummer : ThornreaperCardModel<Midsummer.CardTop, Midsummer.CardBottom>
{
	public override string Name => "Midsummer";
	public override int Level => 1;
	public override int Initiative => 50;
	protected override int AtlasIndex => 6;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(3, new HealSquare(this, new Vector2(0.44158298f, 0.25373963f)))
				.WithRange(3, new RangeSquare(this, new Vector2(0.6519975f, 0.25207758f)))
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Light)];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Light)),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
				.Build())
		];

		public override int XP => 1;
		public override bool Loss => true;
	}
}
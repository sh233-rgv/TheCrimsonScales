using System.Collections.Generic;
using Godot;

public class BurningPit : ChieftainCardModel<BurningPit.CardTop, BurningPit.CardBottom>
{
	public override string Name => "Burning Pit";
	public override int Level => 1;
	public override int Initiative => 45;
	protected override int AtlasIndex => 28 - 0;

	public class CardTop : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithName("Blazing Fire")
				.WithTexturePath("res://Content/Classes/SpiritCaller/Summons/blazing_fire.png")
				.WithHealth(2, new SummonHealthSquare(this, new Vector2(0.44718847f, 0.23893806f)))
				.WithTraits(new MountTrait(), new PushTrait(1))
				.Build()
			),
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.6224222f, 0.723211f)))
				.Build()),
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Earth)];
	}
}
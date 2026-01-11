using System.Collections.Generic;

public class CatastrophicCattle : ChieftainCardModel<CatastrophicCattle.CardTop, CatastrophicCattle.CardBottom>
{
	public override string Name => "Catastrophic Cattle";
	public override int Level => 1;
	public override int Initiative => 88;
	protected override int AtlasIndex => 0;

	public class CardTop : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithName("Fighting Bull")
				.WithTexturePath("res://Content/Classes/Chieftain/Summons/fighting_bull_AI.png")
				.WithHealth(4)
				.WithMove(2)
				.WithAttack(2)
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
			new AbilityCardAbility(MoveAbility.Builder().WithDistance(3).Build()),
		];

		public override IEnumerable<Element> Elements => [Element.Earth];
	}
}
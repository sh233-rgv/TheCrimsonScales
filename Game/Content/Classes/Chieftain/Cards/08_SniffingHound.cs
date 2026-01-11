using System.Collections.Generic;

public class SniffingHound : ChieftainCardModel<SniffingHound.CardTop, SniffingHound.CardBottom>
{
	public override string Name => "Sniffing Hound";
	public override int Level => 1;
	public override int Initiative => 80;
	protected override int AtlasIndex => 8;

	public class CardTop : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithName("Scout Dog")
				.WithTexturePath("res://Content/Classes/Chieftain/Summons/scout_dog_AI.png")
				.WithHealth(4)
				.WithMove(3)
				.WithAttack(1)
				.WithTraits(new TrapDisarmTrait(1))
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
			new AbilityCardAbility(MoveAbility.Builder().WithDistance(5).Build()),
		];
	}
}
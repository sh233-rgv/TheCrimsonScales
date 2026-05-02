using System.Collections.Generic;

public class EtherealCanine : SpiritCallerCardModel<EtherealCanine.CardTop, EtherealCanine.CardBottom>
{
	public override string Name => "Ethereal Canine";
	public override int Level => 1;
	public override int Initiative => 27;
	protected override int AtlasIndex => 28 - 3;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SpawnAbility.Builder()
				.WithName("Phantom Hound")
				.WithTexturePath("res://Content/Classes/SpiritCaller/Summons/phantom_hound.png")
				.WithHealth(1)
				.WithMove(3)
				.WithAttack(2)
				.WithTraits() //TODO
				.Build()
			)
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Dark)];
		public override int XP => 1;
		public override bool Persistent => true;
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			//TODO
		];

		public override bool Round => true;
	}
}
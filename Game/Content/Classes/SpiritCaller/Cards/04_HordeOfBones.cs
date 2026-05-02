using System.Collections.Generic;

public class HordeOfBones : SpiritCallerCardModel<HordeOfBones.CardTop, HordeOfBones.CardBottom>
{
	public override string Name => "Horde of Bones";
	public override int Level => 1;
	public override int Initiative => 79;
	protected override int AtlasIndex => 28 - 4;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SpawnAbility.Builder()
				.WithName("Phantom Hound")
				.WithTexturePath("res://Content/Classes/SpiritCaller/Summons/phantom_hound.png")
				.WithHealth(2)
				.WithMove(2)
				.WithAttack(1)
				.WithRange(2)
				.WithTraits(new TargetsTrait(2), new PierceTrait(1))
				.Build()
			)
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Air)];
		public override int XP => 1;
		public override bool Persistent => true;
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			//TODO
		];
	}
}
using System.Collections.Generic;

public class MidnightFeast : SpiritCallerCardModel<MidnightFeast.CardTop, MidnightFeast.CardBottom>
{
	public override string Name => "Midnight Feast";
	public override int Level => 1;
	public override int Initiative => 80;
	protected override int AtlasIndex => 28 - 6;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(new DynamicInt<HealAbility.State>(state =>
					3 + (state.Performer is Character characterOwner ? Spirit.GetSpirits(characterOwner).Count : 0)))
				.WithRange(3)
				.Build()),
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			//TODO
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
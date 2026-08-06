using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class StripFlesh : IncarnateCardModel<StripFlesh.CardTop, StripFlesh.CardBottom>
{
	public override string Name => "Strip Flesh";
	public override int Level => 1;
	public override int Initiative => 17;
	protected override int AtlasIndex => 0;

	public class CardTop : IncarnateCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(LootAbility.Builder()
				.WithRange(1)
				.Build()),
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(1)
				.WithTarget(Target.Self)
				.WithConditions(Ruinmaw.Empower)
				.Build())
		];
	}

	public class CardBottom : IncarnateCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[

		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
using System.Collections.Generic;

public class DistantRetribution : BombardCardModel<DistantRetribution.CardTop, DistantRetribution.CardBottom>
{
	public override string Name => "Distant Retribution";
	public override int Level => 2;
	public override int Initiative => 12;
	protected override int AtlasIndex => 12;

	public class CardTop : BombardCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(2).WithRange(3).Build())
		];

		public override int XP => 1;
		public override bool Round => true;
	}

	public class CardBottom : BombardCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder().WithDistance(1).Build()),

			new AbilityCardAbility(ShieldAbility.Builder().WithShieldValue(1).Build())
		];

		public override bool Round => true;
	}
}
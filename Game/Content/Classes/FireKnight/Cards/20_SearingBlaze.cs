using System.Collections.Generic;

public class SearingBlaze : FireKnightLevelUpCardModel<SearingBlaze.CardTop, SearingBlaze.CardBottom>
{
	public override string Name => "Searing Blaze";
	public override int Level => 5;
	public override int Initiative => 68;
	protected override int AtlasIndex => 8;

	public class CardTop : FireKnightCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			//TODO
		];
	}

	public class CardBottom : FireKnightCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			//TODO
		];
	}
}
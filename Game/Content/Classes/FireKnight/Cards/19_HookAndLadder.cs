using System.Collections.Generic;

public class HookAndLadder : FireKnightLevelUpCardModel<HookAndLadder.CardTop, HookAndLadder.CardBottom>
{
	public override string Name => "Hook and Ladder";
	public override int Level => 5;
	public override int Initiative => 32;
	protected override int AtlasIndex => 9;

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
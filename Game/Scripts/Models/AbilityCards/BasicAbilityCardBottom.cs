using System.Collections.Generic;

public class BasicAbilityCardBottom : AbilityCardSideModel
{
	protected override List<AbilityCardAbility> GetAbilities() =>
	[
		new AbilityCardAbility(MoveAbility.Builder().WithDistance(2).Build())
	];
}
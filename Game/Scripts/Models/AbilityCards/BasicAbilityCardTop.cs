using System.Collections.Generic;

public class BasicAbilityCardTop : AbilityCardSideModel
{
	protected override List<AbilityCardAbility> GetAbilities() =>
	[
		new AbilityCardAbility(AttackAbility.Builder().WithDamage(2).Build())
	];
}
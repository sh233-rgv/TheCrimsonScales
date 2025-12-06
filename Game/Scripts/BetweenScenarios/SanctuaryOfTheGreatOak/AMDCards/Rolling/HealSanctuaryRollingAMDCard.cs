using System.Collections.Generic;

public class HealSanctuaryRollingAMDCard : SanctuaryCritAMDCardModel
{
	protected override int AtlasIndex => 2;

	public override int? GetValue(AttackAbility.State attackAbilityState) => 1;

	public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
	[
		HealAbility.Builder().WithHealValue(2).WithRange(2).Build()
	];
}
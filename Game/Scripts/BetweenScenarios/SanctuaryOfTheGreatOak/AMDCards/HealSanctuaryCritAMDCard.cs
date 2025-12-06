using System.Collections.Generic;

public class HealSanctuaryCritAMDCard : SanctuaryCritAMDCardModel
{
	protected override int AtlasIndex => 2;

	public override bool RemoveAfterDraw => true;
	public override AMDCardType Type => AMDCardType.Crit;

	public override List<Ability> GetAbilities(AttackAbility.State state) =>
	[
		HealAbility.Builder().WithHealValue(2).WithRange(2).Build()
	];
}
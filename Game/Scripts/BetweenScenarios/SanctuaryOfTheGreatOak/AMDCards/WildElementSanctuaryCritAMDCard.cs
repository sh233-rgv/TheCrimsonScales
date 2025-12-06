using System.Collections.Generic;

public class WildElementSanctuaryCritAMDCard : SanctuaryCritAMDCardModel
{
	protected override int AtlasIndex => 0;

	public override bool RemoveAfterDraw => true;
	public override AMDCardType Type => AMDCardType.Crit;

	public override List<Ability> GetAbilities(AttackAbility.State state) =>
	[
		ConditionAbility.Builder().WithConditions(Conditions.Bless).WithTarget(Target.Allies).WithRange(int.MaxValue).Build()
	];
}
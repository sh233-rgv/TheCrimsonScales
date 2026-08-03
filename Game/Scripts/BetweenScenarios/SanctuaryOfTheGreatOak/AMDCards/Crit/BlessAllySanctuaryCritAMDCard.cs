using System.Collections.Generic;

public class BlessAllySanctuaryCritAMDCard : SanctuaryCritAMDCardModel
{
	public override string GetSimpleString(RichTextParameters richTextParameters) =>
		GetSimpleString(richTextParameters, AMDCardType.Crit, $"{Icons.InlineCondition(Conditions.Bless, richTextParameters)}");

	public override string ToString(RichTextParameters richTextParameters) =>
		GetBasicString(richTextParameters, AMDCardType.Crit,
			extraText: $"{Icons.InlineCondition(Conditions.Bless, richTextParameters)}, {Icons.Inline(Icons.Targets)}1 ally");

	protected override int AtlasIndex => 0;

	public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
	[
		ConditionAbility.Builder().WithConditions(Conditions.Bless).WithTarget(Target.Allies).WithInfiniteRange().Build()
	];
}
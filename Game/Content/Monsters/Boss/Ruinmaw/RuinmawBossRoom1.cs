using System;
using System.Collections.Generic;

public class RuinmawBossRoom1 : RuinmawBoss, IBossMonsterModel
{
	public override string GetSpecial1Description(Monster monster) =>
		$"""
		 Pouncing Predator -
		 Focus the closest enemy that does not have {Icons.Inline(Icons.GetCondition(Conditions.Rupture))}
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move + 3}, {Icons.Inline(Icons.Jump)}
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack - 1}, {Icons.Inline(Icons.Push)}3, {Icons.Inline(Icons.GetCondition(Conditions.Wound1))}, {Icons.Inline(Icons.GetCondition(Conditions.Rupture))}
		 """;

	public override string GetSpecial2Description(Monster monster) =>
		$"""
		 Terrifying Howl -
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move}
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack - 1}, {Icons.Inline(Icons.Targets)}all adjacent enemies, {Icons.Inline(Icons.Push)}1
		 {Icons.Inline(Icons.Push)}1, {Icons.Inline(Icons.Targets)}all enemies within {Icons.Inline(Icons.Range)}2, {Icons.Inline(Icons.GetCondition(Conditions.Muddle))}
		 """;

	public Action<ScenarioCheckEvents.FigureFocusCheck.Parameters> AdjustFocusSpecial1 =>
		parameters => parameters.SetFocusCondition(Conditions.Rupture);

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +3).WithMoveType(MoveType.Jump)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -1).WithPush(3)
			.WithConditions([Conditions.Wound1, Conditions.Rupture]))
	];

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +0)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -1).WithTarget(Target.TargetAll | Target.Enemies).WithPush(1)),
		new MonsterAbilityCardAbility(PushAbility.Builder().WithPush(1).WithTarget(Target.TargetAll | Target.Enemies).WithRange(2)
			.WithConditions(Conditions.Muddle))
	];
}
using System.Collections.Generic;

public class TerrorscaleDrakeRM4Room3 : TerrorscaleDrake
{
	public override string GetSpecial1Description(Monster monster) =>
		$"""
		 Brood Mother's Call -
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move + 1}, {Icons.Inline(Icons.Jump)}
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, only {Icons.Inline(Icons.Range)}4/5, {Icons.Inline(Icons.GetCondition(Conditions.Poison1))}
		 """;

	public override string GetSpecial2Description(Monster monster) =>
		$"""
		 Desperate Roar -
		 {Icons.Inline(Icons.Move)}{monster.Stats.Move - 2}, {Icons.Inline(Icons.Jump)}
		 {Icons.Inline(Icons.Attack)}{monster.Stats.Attack}, {Icons.Inline(Icons.Targets)}all enemies within 3 hexes, {Icons.Inline(Icons.Push)}2, {Icons.Inline(Icons.GetCondition(Conditions.Muddle))}
		 """;

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial1Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, +1).WithMoveType(MoveType.Jump)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, +0)
			.WithRange(5)
			.WithMinRange(4)
			.WithConditions(Conditions.Poison1)),
		new MonsterAbilityCardAbility(ConditionAbility.Builder().WithConditions(Conditions.Strengthen).WithRange(100)
			.WithTarget(Target.Allies | Target.TargetAll))
	];

	public override IEnumerable<MonsterAbilityCardAbility> GetSpecial2Abilities(Monster monster) =>
	[
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.MoveAbility(monster, -2).WithMoveType(MoveType.Jump)),
		new MonsterAbilityCardAbility(MonsterAbilityCardModel.AttackAbility(monster, -1)
			.WithRange(3)
			.WithRangeType(RangeType.Melee)
			.WithTarget(Target.Enemies | Target.TargetAll)
			.WithPush(2)
			.WithConditions(Conditions.Muddle))
	];
}
using System.Collections.Generic;
using System.Linq;

public class BombardAMDCards
{
	public class RollingPierceThree : BombardAMDCardModel
	{
		protected override int AtlasIndex => 0;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override int? Pierce => 3;
	}

	public class PlusZeroPlusThreeIfProjectile : BombardAMDCardModel
	{
		protected override int AtlasIndex => 2;

		public override int? GetValue(AttackAbility.State attackAbilityState) =>
			attackAbilityState.ActionState.ParentActionState != null &&
			attackAbilityState.ActionState.ParentActionState.AbilityStates.Any(potentialProjectileAbility =>
				potentialProjectileAbility is ProjectileAbility.State)
				? +3
				: +0;
	}

	public class PlusTwoImmobilize : BombardAMDCardModel
	{
		protected override int AtlasIndex => 4;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +2;
		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) => [Conditions.Immobilize];
	}

	public class PlusOneRetaliateOne : BombardAMDCardModel
	{
		protected override int AtlasIndex => 6;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			RetaliateAbility.Builder().WithRetaliateValue(1).WithRange(3).Build()
		];
	}

	public class PlusOnePullSelfTowardTarget : BombardAMDCardModel
	{
		protected override int AtlasIndex => 8;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			PullSelfAbility.Builder().WithPullSelfValue(3).WithCustomGetTargets(((state, list) => list.Add(attackAbilityState.Target))).Build()
		];
	}

	public class PlusZeroStrengthenSelf : BombardAMDCardModel
	{
		protected override int AtlasIndex => 10;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			ConditionAbility.Builder().WithConditions(Conditions.Strengthen).WithTarget(Target.Self).Build()
		];
	}

	public class PlusZeroStun : BombardAMDCardModel
	{
		protected override int AtlasIndex => 11;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;
		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) => [Conditions.Stun];
	}

	public class PlusOneWound : BombardAMDCardModel
	{
		protected override int AtlasIndex => 12;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +1;
		public override List<ConditionModel> GetConditionModels(AttackAbility.State attackAbilityState) => [Conditions.Wound1];
	}

	public class RollingPlusZeroShieldOne : BombardAMDCardModel
	{
		protected override int AtlasIndex => 13;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			ShieldAbility.Builder().WithShieldValue(1).Build()
		];
	}

	public class RollingPlusZeroHealOneSelf : BombardAMDCardModel
	{
		protected override int AtlasIndex => 15;
		public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
		public override int? GetValue(AttackAbility.State attackAbilityState) => +0;

		public override List<Ability> GetAbilities(AttackAbility.State attackAbilityState) =>
		[
			HealAbility.Builder().WithHealValue(1).WithTarget(Target.Self).Build()
		];
	}
}
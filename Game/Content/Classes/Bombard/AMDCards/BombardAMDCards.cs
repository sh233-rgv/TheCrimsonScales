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
}
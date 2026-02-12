using System.Collections.Generic;

public class Anticoagulant : MirefootCardModel<Anticoagulant.CardTop, Anticoagulant.CardBottom>
{
	public override string Name => "Anticoagulant";
	public override int Level => 6;
	public override int Initiative => 12;
	protected override int AtlasIndex => 22;

	public class CardTop : MirefootCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(1)
				.WithConditions(Conditions.Wound1)
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithConditions(Conditions.Wound2)
				.Build())
		];
	}

	public class CardBottom : MirefootCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.WithMoveType(MoveType.Jump)
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3)
				.WithMoveType(MoveType.Jump)
				.Build())
		];
	}
}
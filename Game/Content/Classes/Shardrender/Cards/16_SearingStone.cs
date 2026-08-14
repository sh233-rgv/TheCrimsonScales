using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class SearingStone : ShardrenderCardModel<SearingStone.CardTop, SearingStone.CardBottom>
{
	public override string Name => "Searing Stone";
	public override int Level => 3;
	public override int Initiative => 13;
	protected override int AtlasIndex => 16;

	public class CardTop : ShardrenderCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithRange(3)
				.WithConditions([Conditions.Wound1, Conditions.Muddle])
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red)
					]
				))
				.Build())
		];

		public override int XP => 1;
		public override bool Loss => true;
	}

	public class CardBottom : ShardrenderCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackDiamond(this, new Vector2(0.45467615f, 0.67177606f)))
				.WithRange(2)
				.WithConditions(Conditions.Wound1)
				.Build()),
			new AbilityCardAbility(ControlAbility.Builder()
				.WithAbilities(
				[
					MoveAbility.Builder().WithDistance(1).Build()
				])
				.WithConditionalAbilityCheck()
				.Build())
		];

		public override bool Round => true;
	}
}
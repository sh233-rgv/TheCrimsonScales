using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ChillingWave : LuminaryCardModel<ChillingWave.CardTop, ChillingWave.CardBottom>
{
	public override string Name => "Chilling Wave";
	public override int Level => 1;
	public override int Initiative => 39;
	protected override int AtlasIndex => 1;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Empty),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Empty),
					]
				))
				.WithDuringAttackSubscriptions(
				[
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAddCondition(Conditions.Stun);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Stun))}")
					),
				])
				.Build()),
			Scuttle(1, [Element.Ice]),
		];
		protected override int XP => 1;
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(1)
				.Build())
		];
	}
}
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class AstronomicalStrike : BrightsparkCardModel<AstronomicalStrike.CardTop, AstronomicalStrike.CardBottom>
{
	public override string Name => "Astronomical Strike";
	public override int Level => 7;
	public override int Initiative => 57;
	protected override int AtlasIndex => 24;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest).Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red),
					]
				))
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Light,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAdjustAttackValue(1);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Attack)}")
					)
				)
				.Build())
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithSummonStats(new SummonStats()
				{
					Health = 6,
					//TODO: Pull ability
					Range = 4,
					Traits =
					[
						new PerformAtEndOfTurnTrait(OtherAbility.Builder().WithPerformAbility(async state =>
						{
							foreach(Figure figure in RangeHelper.GetFiguresInRange(state.Performer.Hex, 1)
								        .Where(figure => figure.EnemiesWith(state.Performer)))
							{
								await AbilityCmd.SufferDamage(state, figure, 1);
								state.SetPerformed();
							}
						}).Build()),
						//TODO: Scenarios added: new PermanentConditionTrait(Conditions.Invisible),
						//TODO: Cannot be moved
					]
				})
				.WithName("Black Hole")
				.WithTexturePath("res://Content/Classes/Brightspark/BlackHole.png")
				.Build()
			),
		];

		protected override int XP => 2;
		protected override bool Persistent => true;
		public override bool Loss => true;
	}
}
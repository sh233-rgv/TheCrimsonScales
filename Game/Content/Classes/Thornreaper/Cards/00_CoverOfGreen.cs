using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class CoverOfGreen : ThornreaperCardModel<CoverOfGreen.CardTop, CoverOfGreen.CardBottom>
{
	public override string Name => "Cover of Green";
	public override int Level => 1;
	public override int Initiative => 34;
	protected override int AtlasIndex => 0;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.37892142f, 0.3163435f)))
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red),
						]
					), new AOEHexMark(Vector2I.Zero.Add(Direction.SouthWest), this, new Vector2(0.61454624f, 0.37839338f)),
					new AOEHexMark(Vector2I.Zero.Add(Direction.East).Add(Direction.NorthEast), this, new Vector2(0.81409585f, 0.25540167f)))
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2, new MoveSquare(this, new Vector2(0.62115484f, 0.66129583f)))
				.Build()),
			new AbilityCardAbility(ShieldAbility.Builder()
				.WithShieldValue(1)
				.WithAbilityPerformedSubscription(
					ScenarioEvents.AbilityPerformed.Subscription.ConsumeElement(Element.Earth,
						applyFunction: async parameters =>
						{
							await AbilityCmd.InfuseElement(parameters.AbilityState, Element.Earth);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters(Icons.Inline(Icons.GetElement(Element.Earth)))))
				.Build())
		];

		public override bool Round => true;
	}
}
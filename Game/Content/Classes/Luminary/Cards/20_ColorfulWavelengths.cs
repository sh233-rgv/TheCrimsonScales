using System.Collections.Generic;
using Godot;

public class ColorfulWavelengths : LuminaryCardModel<ColorfulWavelengths.CardTop, ColorfulWavelengths.CardBottom>
{
	public override string Name => "Colorful Wavelengths";
	public override int Level => 5;
	public override int Initiative => 83;
	protected override int AtlasIndex => 20;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(5)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.ConsumeElement(Element.Fire,
						applyFunction: async parameters =>
						{
							((AttackAbility.State)parameters.AbilityState).AbilitySetAOEPattern(new AOEPattern(
								[
									new AOEHex(Vector2I.Zero, AOEHexType.Gray),
									new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Empty),
									new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.East), AOEHexType.Red),
								]
							));

							await AbilityCmd.InfuseElement(Element.Ice, parameters.AbilityState.Authority, parameters.AbilityState);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"Add the shown area of effect")
					)
				)
				.WithDuringAttackSubscriptions(
				[
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Ice,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAdjustPierce(2);

							await AbilityCmd.InfuseElement(Element.Fire, parameters.AbilityState.Authority, parameters.AbilityState);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.Pierce)}2")
					),
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Light,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAdjustPush(2);

							await AbilityCmd.InfuseElement(Element.Dark, parameters.AbilityState.Authority, parameters.AbilityState);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.Push)}2")
					),
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAddCondition(Conditions.Curse);

							await AbilityCmd.InfuseElement(Element.Light, parameters.AbilityState.Authority, parameters.AbilityState);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Curse))}")
					)
				])
				.Build())
		];
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
					]
				))
				.Build()),
			PerformGlow()
		];

		protected override IEnumerable<Element> Elements => [Element.Fire, Element.Ice, Element.Light, Element.Dark];
		protected override int XP => 2;
		protected override bool Loss => true;
	}
}
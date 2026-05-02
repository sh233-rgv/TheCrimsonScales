using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ToxicCharm : SpiritCallerCardModel<ToxicCharm.CardTop, ToxicCharm.CardBottom>
{
	public override string Name => "Toxic Charm";
	public override int Level => 1;
	public override int Initiative => 59;
	protected override int AtlasIndex => 28 - 7;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(1)
				.WithRange(3)
				.WithConditions(Conditions.Poison1)
				.WithAOEPattern(new AOEPattern(
				[
					new AOEHex(Vector2I.Zero, AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red),
				]))
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Air,
						applyFunction: async parameters =>
						{
							await AbilityCmd.InfuseElement(parameters.AbilityState, Element.Dark);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"Infuse {Icons.Inline(Icons.GetElement(Element.Dark))}")
					))
				.Build()),
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3)
				.Build()),

			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(2)
				.WithTarget(Target.Self)
				.WithConditionalAbilityCheck(async state =>
				{
					await GDTask.CompletedTask;

					return state.Performer.Hex.HasHexObjectOfType<Spirit>();
				})
				.Build()),
		];
	}
}
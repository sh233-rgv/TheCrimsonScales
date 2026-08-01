using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class BrightSkies : ThornreaperCardModel<BrightSkies.CardTop, BrightSkies.CardBottom>
{
	public override string Name => "Bright Skies";
	public override int Level => 3;
	public override int Initiative => 29;
	protected override int AtlasIndex => 16;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.49745992f, 0.24321333f)))
				.WithPierce(1)
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.New(
						_ => LightStrongOrWaning,
						async parameters =>
						{
							parameters.AbilityState.AbilityAdjustAttackValue(1);
							parameters.AbilityState.AbilityAdjustPierce(1);

							await AbilityCmd.InfuseElement(parameters.AbilityState, Element.Light);
						}, canApplyMultipleTimesDuringSubscription: false))
				.Build())
		];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					await AbilityCmd.RemoveOneNegativeCondition(state, state.Performer);
				})
				.WithCanPerformWhileStunned(true)
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.62163085f, 0.80221605f)))
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Light)];
	}
}
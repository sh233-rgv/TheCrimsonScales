using System.Collections.Generic;
using System.Linq;

public class BlindingLightwaves : BrightsparkCardModel<BlindingLightwaves.CardTop, BlindingLightwaves.CardBottom>
{
	public override string Name => "Blinding Lightwaves";
	public override int Level => 1;
	public override int Initiative => 68;
	protected override int AtlasIndex => 1;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Stun)
				.WithRange(2)
				.WithTarget(Target.Enemies | Target.TargetAll)
				.Build())
		];

		protected override IEnumerable<Element> Elements => [Element.Light];
		protected override int XP => 2;
		public override bool Loss => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					foreach(Figure figure in RangeHelper.GetFiguresInRange(state.Performer.Hex, 1)
						        .Where(figure => figure.EnemiesWith(state.Performer)))
					{
						await AbilityCmd.SufferDamage(state, figure, 1);
					}
				}).Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(1)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.ConsumeElement(Element.Light,
						applyFunction: async parameters =>
						{
							AbilityState state = parameters.AbilityState;
							await AbilityCmd.AddCondition(state, state.Performer, Conditions.Invisible);

							state.SetPerformed();
							state.SetBlocked();
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters(
							$"Gain {Icons.Inline(Icons.GetCondition(Conditions.Invisible))} instead")
					))
				.Build())
		];
	}
}
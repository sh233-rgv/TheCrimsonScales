using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ChainsOfLightning : HierophantLevelUpCardModel<ChainsOfLightning.CardTop, ChainsOfLightning.CardBottom>
{
	public override string Name => "Chains Of Lightning";
	public override int Level => 1;
	public override int Initiative => 31;
	protected override int AtlasIndex => 15 - 9;

	public class CardTop : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(PullAbility.Builder()
				.WithPull(2)
				.WithTargets(2)
				.WithRange(3)
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithTarget(Target.TargetAll | Target.Enemies)
				.Build())
		];
	}

	public class CardBottom : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithRange(4)
				.WithAfterTargetConfirmedSubscription(
					ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
						parameters => RangeHelper.GetFiguresInRange(parameters.AbilityState.Target.Hex, 1).Any(figure => figure.AlliedWith(parameters.AbilityState.Performer)),
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAddCondition(Conditions.Wound1);

							await GDTask.CompletedTask;
						}
					)
				)
				.Build()),

			new AbilityCardAbility(GrantAbility.Builder()
				.WithGetAbilities(state =>
				[
					ShieldAbility.Builder()
						.WithShieldValue(2)
						.WithConditionalAbilityCheck(state => AbilityCmd.AskConsumeElement(state.Performer, Element.Earth))
						.WithOnAbilityEndedPerformed(async state =>
						{
							await GDTask.CompletedTask;

							state.ActionState.SetOverrideRound();
						})
						.Build()
				])
				.WithCustomGetTargets((state, list) =>
				{
					AttackAbility.State attackAbilityState = state.ActionState.GetAbilityState<AttackAbility.State>(0);

					foreach(Figure target in attackAbilityState.UniqueTargetedFigures)
					{
						list.AddRange(RangeHelper.GetFiguresInRange(target.Hex, 1));
					}
				})
				.WithConditionalAbilityCheck(async state =>
					{
						await GDTask.CompletedTask;

						AttackAbility.State attackAbilityState = state.ActionState.GetAbilityState<AttackAbility.State>(0);

						return attackAbilityState.Performed;
					}
				)
				.Build())
		];
	}
}
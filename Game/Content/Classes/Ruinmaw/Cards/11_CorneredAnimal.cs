using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class CorneredAnimal : RuinmawCardModel<CorneredAnimal.CardTop, CorneredAnimal.CardBottom>
{
	public override string Name => "Cornered Animal";
	public override int Level => 1;
	public override int Initiative => 15;
	protected override int AtlasIndex => 11;

	public class CardTop : RuinmawCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(1)
				.WithTarget(Target.Self)
				.WithConditions(Ruinmaw.Empower)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New(
						parameters => IsSated(parameters.Performer),
						async parameters =>
						{
							((HealAbility.State)parameters.AbilityState).AbilityAddCondition(Conditions.Ward);
							await GDTask.CompletedTask;
						}
					)
				)
				.WithOnAbilityEndedPerformed(async state =>
					{
						if(IsSated(state.Performer))
						{
							await AbilityCmd.GainXP(state.Performer, 1);
						}
					}
				)),

			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AfterAttackPerformedEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.AbilityState.Target == state.Performer,
						async parameters =>
						{
							ActionState actionState = new ActionState(state.Performer, [AttackAbility.Builder().WithDamage(3)]);
							await actionState.Perform();

							await state.ActionState.RequestDiscardOrLose();
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AfterAttackPerformedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				}))
		];

		public override bool Round => true;
	}

	public class CardBottom : RuinmawCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(1)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New(
						parameters => RangeHelper.GetHexesInRange(parameters.Performer.Hex, 1).Any(hex => hex.HasHexObjectOfType<Obstacle>()),
						async parameters =>
						{
							((AttackAbility.State)parameters.AbilityState).AbilitySetHasAdvantage();
							await AbilityCmd.GainXP(parameters.Performer, 1);
						}
					)
				)),
			new AbilityCardAbility(ShieldAbility.Builder()
				.WithShieldValue(1))
		];

		public override bool Round => true;
	}
}
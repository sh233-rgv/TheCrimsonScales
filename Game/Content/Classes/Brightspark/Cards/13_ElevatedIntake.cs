using System.Collections.Generic;
using Fractural.Tasks;

public class ElevatedIntake : BrightsparkCardModel<ElevatedIntake.CardTop, ElevatedIntake.CardBottom>
{
	public override string Name => "Elevated Intake";
	public override int Level => 1;
	public override int Initiative => 50;
	protected override int AtlasIndex => 13;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(5)
				.WithRange(3)
				.WithOnAbilityStarted(async state =>
				{
					ScenarioEvents.ItemStateChangedEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Item.IsConsumed() && canApplyParameters.Item.Owner == state.Performer,
						async applyParameters =>
						{
							state.AbilityAdjustAttackValue(2);
							await AbilityCmd.GainXP(state.Performer, 1);
							ScenarioEvents.ItemStateChangedEvent.Unsubscribe(state, this);
						}
					);
					await GDTask.CompletedTask;
				})
				.WithOnAbilityEnded(async abilityState =>
					{
						ScenarioEvents.ItemStateChangedEvent.Unsubscribe(abilityState, this);

						await GDTask.CompletedTask;
					}
				)
				.Build())
		];

		protected override int XP => 1;
		public override bool Loss => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3)
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.ItemStateChangedEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Item.Owner == state.Performer && canApplyParameters.Item.IsConsumed(),
						async applyParameters =>
						{
							//TODO: Add state to infuse
							await AbilityCmd.InfuseWildElement(state.Performer);
							await state.ActionState.RequestDiscardOrLose();
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.ItemStateChangedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.Build())
		];

		protected override bool Round => true;
	}
}
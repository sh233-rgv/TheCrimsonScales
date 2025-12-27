using System.Collections.Generic;
using Fractural.Tasks;

public class NutrientOverdose : BrightsparkCardModel<NutrientOverdose.CardTop, NutrientOverdose.CardBottom>
{
	public override string Name => "Nutrient Overdose";
	public override int Level => 2;
	public override int Initiative => 17;
	protected override int AtlasIndex => 14;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.Build())
		];
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
							canApplyParameters.Item.Owner == state.Performer && canApplyParameters.Item.ItemState == ItemState.Consumed,
						async applyParameters =>
						{
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
using System.Collections.Generic;
using Fractural.Tasks;

public class EnvironmentalSurvey : BrightsparkCardModel<EnvironmentalSurvey.CardTop, EnvironmentalSurvey.CardBottom>
{
	public override string Name => "Environmental Survey";
	public override int Level => 1;
	public override int Initiative => 31;
	protected override int AtlasIndex => 11;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			//TODO: Top ability
		];

		protected override IEnumerable<Element> Elements => [Element.Earth];
		protected override int XP => 1;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					IEnumerable<AbilityCard> selectedAbilityCards =
						await AbilityCmd.SelectAbilityCards((Character)state.Performer, CardState.Discarded, 0, 3,
							hintText: $"Select up to 3 cards to recover");

					foreach(AbilityCard selectedAbilityCard in selectedAbilityCards)
					{
						await AbilityCmd.ReturnToHand(selectedAbilityCard);

						state.SetPerformed();
					}
				})
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.ShortRestStartedEvent.Subscribe(state, this,
						parameters => parameters.Character == state.Performer,
						async parameters =>
						{
							parameters.SetCanSelectCardToUse();
							ItemModel item = await AbilityCmd.SelectItem(parameters.Character, ItemState.Spent,
								hintText: "Select an item to recover");
							if(item != null)
							{
								await item.Refresh();
							}
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.ShortRestStartedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.Build())
		];

		//TODO: Add any element
		//protected override IEnumerable<Element> Elements => WildElement;
		protected override int XP => 2;
		protected override bool Persistent => true;
		public override bool Loss => true;
	}
}
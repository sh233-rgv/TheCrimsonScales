using System.Collections.Generic;
using Fractural.Tasks;

public class PreliminaryResearch : BrightsparkCardModel<PreliminaryResearch.CardTop, PreliminaryResearch.CardBottom>
{
	public override string Name => "Preliminary Research";
	public override int Level => 1;
	public override int Initiative => 24;
	protected override int AtlasIndex => 10;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithTargets(2)
				.WithRange(3)
				.Build())
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
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

		//TODO: public override IEnumerable<Element> Elements => WildElement;
		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
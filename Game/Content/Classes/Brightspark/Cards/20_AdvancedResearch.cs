using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class AdvancedResearch : BrightsparkCardModel<AdvancedResearch.CardTop, AdvancedResearch.CardBottom>
{
	public override string Name => "Advanced Research";
	public override int Level => 5;
	public override int Initiative => 20;
	protected override int AtlasIndex => 85;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					//TODO: Waiting for luminary
					await GDTask.CompletedTask;
				})
				.WithAbilityStartedSubscriptions(
				[
					//TODO: Need Consume Any from Luminary
				])
				.Build())
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					List<AbilityCard> selectedAbilityCards =
						await AbilityCmd.SelectAbilityCards(state.Performer as Character, CardState.Discarded, 0, 2,
							hintText: $"Select up to two discarded cards to recover");

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
					ScenarioEvents.LongRestCardSelectionEvent.Subscribe(state, this,
						parameters => parameters.Character == state.Performer,
						async parameters =>
						{
							//TODO: Hierophant L9
						});
				})
				.WithOnDeactivate(async state =>
				{
				})
				.Build())
		];

		//TODO: 2 Any Elements
		protected override int XP => 2;
		protected override bool Persistent => true;
		public override bool Loss => true;
	}
}
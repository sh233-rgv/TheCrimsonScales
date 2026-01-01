using System.Collections.Generic;
using Fractural.Tasks;

public class Electromagnetism : BrightsparkCardModel<Electromagnetism.CardTop, Electromagnetism.CardBottom>
{
	public override string Name => "Electromagnetism";
	public override int Level => 3;
	public override int Initiative => 84;
	protected override int AtlasIndex => 17;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithSummonStats(new SummonStats()
				{
					Health = 6,
					Move = 1,
					Traits =
					[
						new PerformAtEndOfTurnTrait(LootAbility.Builder().WithRange(1)
							.WithCustomGetLootObtainer(state => ((Summon)state.Performer).CharacterOwner).Build())
					]
				})
				.WithName("Metal Detector")
				.WithTexturePath("res://Content/Classes/Brightspark/MetalDetector.png")
				.Build()
			),
		];

		protected override int XP => 2;
		protected override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(PullAbility.Builder()
				.WithPull(2)
				.WithRange(3)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.ConsumeElement(Element.Air,
						applyFunction: async applyParameters =>
						{
							((PullAbility.State)applyParameters.AbilityState).AbilityAdjustRange(1);
							((PullAbility.State)applyParameters.AbilityState).AbilityAdjustPull(1);

							await GDTask.CompletedTask;
						}))
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithPush(1)
				.Build())
		];
	}
}
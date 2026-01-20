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
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithName("Metal Detector")
				.WithTexturePath("res://Content/Classes/Brightspark/MetalDetector.png")
				.WithHealth(6)
				.WithMove(1)
				.WithTraits(new PerformAtEndOfTurnTrait(LootAbility.Builder().WithRange(1)
					.WithCustomGetLootObtainer(state => ((Summon)state.Performer).CharacterOwner).Build()))
				.Build()
			),
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
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
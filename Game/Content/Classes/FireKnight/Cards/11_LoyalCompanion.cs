using System.Collections.Generic;
using Fractural.Tasks;

public class LoyalCompanion : FireKnightCardModel<LoyalCompanion.CardTop, LoyalCompanion.CardBottom>
{
	public override string Name => "Loyal Companion";
	public override int Level => 1;
	public override int Initiative => 82;
	protected override int AtlasIndex => 12 - 11;

	public class CardTop : FireKnightCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithName("Spotted Hound")
				.WithTexturePath("res://Content/Classes/FireKnight/SpottedHound.jpg")
				.WithHealth(4)
				.WithMove(2)
				.WithAttack(1)
				.WithTraits(new AdjacentAlliesGainAdvantageTrait())
				.Build()
			),

			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					SummonAbility.State summonAbilityState = state.ActionState.GetAbilityState<SummonAbility.State>(0);
					ActionState actionState = new ActionState(state.ActionState, summonAbilityState.Summon,
					[
						ConditionAbility.Builder()
							.WithConditions(Conditions.Bless)
							.WithTarget(Target.Allies | Target.TargetAll)
							.WithRange(2)
							.Build()
					]);
					await actionState.Perform();

					state.SetPerformed();
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build())
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : FireKnightCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New(
						parameters => parameters.Performer.Hex.HasHexObjectOfType<Ladder>(),
						async parameters =>
						{
							MoveAbility.State moveAbilityState = (MoveAbility.State)parameters.AbilityState;
							moveAbilityState.AddJump();

							await GDTask.CompletedTask;
						}
					)
				)
				.Build())
		];
	}
}
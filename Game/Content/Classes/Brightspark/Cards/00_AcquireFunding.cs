using System.Collections.Generic;

public class AcquireFunding : BrightsparkCardModel<AcquireFunding.CardTop, AcquireFunding.CardBottom>
{
	public override string Name => "Acquire Funding";
	public override int Level => 1;
	public override int Initiative => 61;
	protected override int AtlasIndex => 0;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithRange(3)
				.WithAfterAttackPerformedSubscription(
					ScenarioEvents.AfterAttackPerformed.Subscription.New(
						parameters => parameters.AbilityState.Target.IsDead,
						async parameters =>
                        {
                            
                        })
				)
				.Build())
		];

		protected override int XP => 1;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Wound1, Conditions.Immobilize)
				.WithRange(2)
				.Build())
		];
	}
}
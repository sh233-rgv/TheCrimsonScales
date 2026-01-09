using System.Collections.Generic;
using Fractural.Tasks;

public class SufferingSteel : ChainguardLevelUpCardModel<SufferingSteel.CardTop, SufferingSteel.CardBottom>
{
	public override string Name => "Suffering Steel";
	public override int Level => 6;
	public override int Initiative => 09;
	protected override int AtlasIndex => 15 - 8;

	public class CardTop : ChainguardCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(4).Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						canApply: canApplyParameters => canApplyParameters.FromAttack &&
						                                canApplyParameters.Figure == state.Performer &&
						                                canApplyParameters.PotentialAbilityState.Performer.HasCondition(Chainguard.Shackle),
						async applyParameters =>
						{
							applyParameters.SetDamagePrevented();

							await GDTask.CompletedTask;
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 2;
		public override bool Round => true;
		public override bool Loss => true;
	}

	public class CardBottom : ChainguardCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder().WithDistance(2).Build()),
			new AbilityCardAbility(RetaliateAbility.Builder().WithRetaliateValue(2).Build()),
		];

		public override bool Round => true;
	}
}
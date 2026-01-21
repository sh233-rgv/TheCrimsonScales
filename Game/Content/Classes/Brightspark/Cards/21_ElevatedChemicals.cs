using System.Collections.Generic;
using Fractural.Tasks;

public class ElevatedChemicals : BrightsparkCardModel<ElevatedChemicals.CardTop, ElevatedChemicals.CardBottom>
{
	public override string Name => "Elevated Chemicals";
	public override int Level => 5;
	public override int Initiative => 44;
	protected override int AtlasIndex => 21;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithDuringAttackSubscriptions(
					[
						ScenarioEvents.DuringAttack.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								parameters.AbilityState.AbilityAdjustAttackValue(2);

								await GDTask.CompletedTask;
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"+2{Icons.Inline(Icons.Attack)}")
						),
						ScenarioEvents.DuringAttack.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								parameters.AbilityState.AbilityAdjustAttackValue(1);

								await AbilityCmd.GainXP(parameters.Performer, 1);
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Attack)}")
						),
						ScenarioEvents.DuringAttack.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								parameters.AbilityState.AbilityAddCondition(Conditions.Disarm);

								await AbilityCmd.GainXP(parameters.Performer, 1);
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Disarm))}"),
							elementsToConsume: 2
						)
					]
				)
				.Build())
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
				.WithDuringMovementSubscriptions(
					[
						ScenarioEvents.DuringMovement.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								parameters.AbilityState.AddJump();

								await GDTask.CompletedTask;
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.Jump)}")
						),
						ScenarioEvents.DuringMovement.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								parameters.AbilityState.AdjustMoveValue(2);

								await AbilityCmd.GainXP(parameters.Performer, 1);
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"+2{Icons.Inline(Icons.Move)}")
						),
						ScenarioEvents.DuringMovement.Subscription.ConsumeWildElements(
							applyFunction: async parameters =>
							{
								await AbilityCmd.InfuseWildElement(parameters.AbilityState);
							},
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.WildElement)}")
						)
					]
				)
				.Build())
		];
	}
}
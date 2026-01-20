using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class MolecularHydroblast : BrightsparkCardModel<MolecularHydroblast.CardTop, MolecularHydroblast.CardBottom>
{
	public override string Name => "Molecular Hydroblast";
	public override int Level => 6;
	public override int Initiative => 29;
	protected override int AtlasIndex => 23;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(5)
				.WithRange(3)
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Ice,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAddCondition(Conditions.Immobilize);

							await AbilityCmd.GainXP(parameters.Performer, 1);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Immobilize))}")
					)
				)
				.Build())
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(5)
				.WithMoveType(MoveType.Jump)
				.WithDuringMovementSubscription(
					ScenarioEvents.DuringMovement.Subscription.ConsumeElements([Element.Fire, Element.Ice],
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AdjustMoveValue(-1);
							parameters.AbilityState.SetCustomValue(this, "ElementsConsumed", true);
							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters(
							$"-1{Icons.Inline(Icons.Move)}, all enemies moved through gain {Icons.Inline(Icons.GetCondition(Conditions.Immobilize))}")))
				.WithAbilityPerformedSubscription(
					ScenarioEvents.AbilityPerformed.Subscription.New(parameters =>
							parameters.AbilityState.GetCustomValue<bool>(this, "ElementsConsumed"),
						async parameters =>
						{
							MoveAbility.State moveAbilityState = ((MoveAbility.State)parameters.AbilityState);
							foreach(Figure figure in moveAbilityState.Hexes
								        .SelectMany(hex => hex.GetHexObjectsOfType<Figure>()).Distinct()
								        .Where(f => moveAbilityState.Performer.EnemiesWith(f)))
							{
								await AbilityCmd.AddCondition(moveAbilityState, figure, Conditions.Immobilize);
							}
						})
				)
				.Build())
		];
	}
}
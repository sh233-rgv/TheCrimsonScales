using System.Collections.Generic;
using Godot;
using Fractural.Tasks;

public class StoneMeteorite : StarslingerCardModel<StoneMeteorite.CardTop, StoneMeteorite.CardBottom>
{
	public override string Name => "Stone Meteorite";
	public override int Level => 7;
	public override int Initiative => 90;
	protected override int AtlasIndex => 24;

	public class CardTop : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(6)
				.WithRange(5)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New(
						parameters => !parameters.Performer.IsDamaged(),
						async parameters =>
						{
							((AttackAbility.State)parameters.AbilityState).AbilitySetAOEPattern(new AOEPattern([
								new AOEHex(Vector2I.Zero, AOEHexType.Red),
								new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
							]));
							await AbilityCmd.GainXP(parameters.Performer, 1);
						}
					)
				)
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAddCondition(Conditions.Stun);
							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Stun))}")
				))
				.Build()),
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(2)
				.WithTarget(Target.Allies | Target.TargetAll)
				.WithCustomGetTargets((abilityState, list) =>
				{
					ConditionAbility.State conditionAbilityState = abilityState.ActionState.GetAbilityState<ConditionAbility.State>(0);

					if(conditionAbilityState.Performed)
					{
						foreach(Hex yellowHex in conditionAbilityState.GetYellowAOEHexes())
						{
							foreach(Figure figure in yellowHex.GetHexObjectsOfType<Figure>())
							{
								list.Add(figure);
							}
						}
					}
				})
				.Build())
		];

		protected override int XP => 1;
	}

	public class CardBottom : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(5)
				.WithDuringMovementSubscription(
					ScenarioEvents.DuringMovement.Subscription.ConsumeElement(Element.Light,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AdjustMoveValue(2);
							parameters.AbilityState.AdjustMoveType(MoveType.Jump);

							await AbilityCmd.GainXP(parameters.Performer, 1);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+2{Icons.Inline(Icons.Move)}, {Icons.Inline(Icons.Jump)}")
					)
				)
				.Build()),
		];
	}
}
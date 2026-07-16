using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class Kleptotherapy : RimehearthCardModel<Kleptotherapy.CardTop, Kleptotherapy.CardBottom>
{
	public override string Name => "Kleptotherapy";
	public override int Level => 1;
	public override int Initiative => 48;
	protected override int AtlasIndex => 8;

	public class CardTop : RimehearthCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackDiamond(this, new Vector2(0.3049949f, 0.23268698f)))
				.WithRange(2)
				.WithRangeType(RangeType.Melee)
				.WithConditions([Conditions.Wound1, Conditions.Chill])
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement([CardElementConsumption.ConsumeWild()],
						applyFunction: async applyParameters =>
						{
							if(applyParameters.Performer.TryGetCondition(Conditions.Chill, out Condition chill))
							{
								applyParameters.AbilityState.AbilityAdjustAttackValue(chill.StackCount);
							}

							applyParameters.AbilityState.AbilitySetHasAdvantage();

							await AbilityCmd.GainXP(applyParameters.Performer, 1);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters(
							$"This attack is unaffected by {Icons.Inline(Icons.GetCondition(Conditions.Chill))}, advantage")
					)
				)
				.Build()),
		];
	}

	public class CardBottom : RimehearthCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Chill)
				.WithTarget(Target.Self)
				.WithMandatory(true)
				.Build()),
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer,
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAdjustAttackValue(2 + state.GetCustomValue<int>(this, "IceConsumed"));

							await GDTask.CompletedTask;
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithUseSlot(new UseSlot(new Vector2(0.49901202f, 0.8941828f), GainXP))
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.ConsumeElement(Element.Ice,
						applyFunction: async applyParameters =>
						{
							applyParameters.AbilityState.SetCustomValue(this, "IceConsumed", 1);
							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+3{Icons.Inline(Icons.Attack)} instead")
					)
				)
				.Build())
		];

		public override bool Persistent => true;
	}
}
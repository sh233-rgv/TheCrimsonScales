using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class StrengthElixir : BrightsparkCardModel<StrengthElixir.CardTop, StrengthElixir.CardBottom>
{
	public override string Name => "Strength Elixir";
	public override int Level => 4;
	public override int Initiative => 19;
	protected override int AtlasIndex => 19;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(GrantAbility.Builder()
				.WithGetAbilities(state =>
				[
					ConditionAbility.Builder()
						.WithConditions(Conditions.Strengthen)
						.WithTarget(Target.Self)
						.Build(),
					ShieldAbility.Builder()
						.WithShieldValue(1)
						.Build()
				])
				.WithRange(2)
				.WithTarget(Target.SelfOrAllies | Target.TargetAll)
				.Build())
		];

		protected override bool Round => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						parameters => parameters.AbilityState.Target == state.Performer,
						async parameters =>
						{
							ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
								canApplyParameters => canApplyParameters.Figure.AlliedWith(state.Performer, true) && canApplyParameters.FromAttack &&
								                      canApplyParameters.PotentialAbilityState.ActionState == parameters.AbilityState.ActionState,
								async applyParameters =>
								{
									applyParameters.AdjustShield(1);

									await GDTask.CompletedTask;
								}
							);
							
							ScenarioEvents.ActionEndedEvent.Subscribe(state, this,
								canApplyParameters => canApplyParameters.ActionState == parameters.AbilityState.ActionState,
								async applyParameters =>
								{
									ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);
									ScenarioEvents.AbilityEndedEvent.Unsubscribe(state, this);
									
									await GDTask.CompletedTask;
								});

							await state.AdvanceUseSlot();
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					//TODO: Fix Use slot positioning
					new UseSlot(new Vector2(0.16650043f, 0.3549993f)),
					new UseSlot(new Vector2(0.57749975f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP)
				])
				.Build())
		];

		protected override bool Persistent => true;
	}
}
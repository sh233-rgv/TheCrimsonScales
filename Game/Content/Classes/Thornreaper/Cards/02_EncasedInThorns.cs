using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class EncasedInThorns : ThornreaperCardModel<EncasedInThorns.CardTop, EncasedInThorns.CardBottom>
{
	public override string Name => "Encased in Thorns";
	public override int Level => 1;
	public override int Initiative => 22;
	protected override int AtlasIndex => 2;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(RetaliateAbility.Builder()
				.WithRetaliateValue(1, new RetaliateSquare(this, new Vector2(0.49891204f, 0.1730072f)))
				.WithRange(2)
				.WithAbilityStartedSubscription(
					ScenarioEvents.AbilityStarted.Subscription.New(
						parameters => parameters.Performer.Hex.HasHexObjectOfType<HazardousTerrain>(),
						async parameters =>
						{
							((RetaliateAbility.State)parameters.AbilityState).AdjustRetaliateValue(1);

							await AbilityCmd.GainXP(parameters.Performer, 1);
						}))
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioCheckEvents.ShieldCheckEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer && LightStrongOrWaning,
						parameters =>
						{
							parameters.AdjustShield(1);
						}
					);

					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer && parameters.FromAttack && LightStrongOrWaning,
						async parameters =>
						{
							parameters.AdjustShield(1);

							await GDTask.CompletedTask;
						}
					);

					ScenarioEvents.FinishElementConsumedEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.ConsumedElement == Element.Light,
						async _ =>
						{
							ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();
							await GDTask.CompletedTask;
						}, EffectType.Visuals);

					ScenarioEvents.FinishElementInfusedEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.InfusedElement == Element.Light,
						async _ =>
						{
							ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();

							await GDTask.CompletedTask;
						}, EffectType.Visuals);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(state, this);
					ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);
					ScenarioEvents.FinishElementConsumedEvent.Unsubscribe(state, this);
					ScenarioEvents.FinishElementInfusedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override bool Round => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.62115484f, 0.63102496f)))
				.WithOnAbilityStarted(async abilityState =>
				{
					ScenarioCheckEvents.MoveCheckEvent.Subscribe(abilityState, this,
						canApplyParameters => canApplyParameters.AbilityState == abilityState &&
						                      canApplyParameters.Hex.HasHexObjectOfType<HazardousTerrain>(),
						applyParameters =>
						{
							applyParameters.SetAffectedByNegativeHex(false);
						}
					);

					ScenarioEvents.HazardousTerrainTriggeredEvent.Subscribe(abilityState, this,
						canApplyParameters => canApplyParameters.PotentialAbilityState?.Performer == abilityState.Performer,
						async applyParameters =>
						{
							applyParameters.SetAffectedByHazardousTerrain(false);
							await GDTask.CompletedTask;
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnAbilityEnded(async abilityState =>
					{
						ScenarioCheckEvents.MoveCheckEvent.Unsubscribe(abilityState, this);
						ScenarioEvents.HazardousTerrainTriggeredEvent.Unsubscribe(abilityState, this);

						await GDTask.CompletedTask;
					}
				)
				.Build()),
			new AbilityCardAbility(ShieldAbility.Builder()
				.WithShieldValue(1)
				.WithConditionalAbilityCheck(async state =>
				{
					await GDTask.CompletedTask;

					return state.Performer.Hex.HasHexObjectOfType<HazardousTerrain>();
				})
				.WithOnAbilityEndedPerformed(async state =>
				{
					state.ActionState.SetOverrideRound();

					await GDTask.CompletedTask;
				})
				.Build())
		];
	}
}
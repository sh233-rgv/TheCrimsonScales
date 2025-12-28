using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class FrostyGlimmer : LuminaryCardModel<FrostyGlimmer.CardTop, FrostyGlimmer.CardBottom>
{
	public override string Name => "Frosty Glimmer";
	public override int Level => 1;
	public override int Initiative => 55;
	protected override int AtlasIndex => 3;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(2)
				.WithRange(3)
				.WithDuringHealSubscriptions(
				[
					ScenarioEvents.DuringHeal.Subscription.ConsumeElement(Element.Ice,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAdjustHealValue(1);
							parameters.AbilityState.AbilityAdjustRange(1);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Heal)}, +1{Icons.Inline(Icons.Range)}")
					),
					ScenarioEvents.DuringHeal.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AdjustTargets(1);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Targets)}")
					)
				])
				.Build())
		];

		protected override int XP => 1;
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3)
				.WithDuringMovementSubscriptions(
				[
					ScenarioEvents.DuringMovement.Subscription.ConsumeElement(Element.Fire,
						applyFunction: async parameters =>
						{
							ScenarioCheckEvents.MoveCheckEvent.Subscribe(parameters.AbilityState, this,
								canApplyParameters =>
									canApplyParameters.AbilityState == parameters.AbilityState && canApplyParameters.Hex.HasHexObjectOfType<Trap>(),
								applyParameters =>
								{
									if(applyParameters.Hex.HasHexObjectOfType<Trap>())
									{
										applyParameters.SetAffectedByNegativeHex(false);
									}
								}
							);

							ScenarioEvents.TrapTriggeredEvent.Subscribe(parameters.AbilityState, this,
								canApplyParameters => canApplyParameters.PotentialAbilityState?.Performer == parameters.AbilityState.Performer,
								async applyParameters =>
								{
									applyParameters.SetTriggersTrap(false);
									await GDTask.CompletedTask;
								}
							);

							ScenarioEvents.AbilityEndedEvent.Subscribe(parameters.AbilityState, this,
								canApplyParameters => canApplyParameters.AbilityState == parameters.AbilityState,
								async applyParameters =>
								{
									ScenarioEvents.AbilityEndedEvent.Unsubscribe(parameters.AbilityState, this);
									ScenarioEvents.TrapTriggeredEvent.Unsubscribe(parameters.AbilityState, this);
									ScenarioCheckEvents.MoveCheckEvent.Unsubscribe(parameters.AbilityState, this);

									Hex hex = await AbilityCmd.SelectHex(parameters.AbilityState, list =>
									{
										list.AddRange(parameters.AbilityState.Hexes.Where(hex => hex.HasHexObjectOfType<Trap>()));
									}, hintText: "Select a trap to destroy");
									if(hex != null)
									{
										await hex.GetHexObjectOfType<Trap>().Destroy();
									}
								}
							);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters("Ignore traps, destroy one move through")
					),
					ScenarioEvents.DuringMovement.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AdjustMoveValue(1);
							parameters.AbilityState.AdjustMoveType(MoveType.Jump);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Move)}, {Icons.Inline(Icons.Jump)}")
					)
				])
				.Build())
		];
	}
}
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class LeftoverTonic : BrightsparkCardModel<LeftoverTonic.CardTop, LeftoverTonic.CardBottom>
{
	public override string Name => "Leftover Tonic";
	public override int Level => 1;
	public override int Initiative => 70;
	protected override int AtlasIndex => 12;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer &&
						              parameters.Figure.TurnPerformedActionStates.SelectMany(actionState => actionState.AbilityStates)
							              .All(abilityState => abilityState is not AttackAbility.State || !abilityState.Performed),
						async parameters =>
						{
							ActionState actionState = new ActionState(parameters.Figure,
							[
								AttackAbility.Builder().WithDamage(2).WithRange(2).Build(),
							]);
							await actionState.Perform();
							await state.AdvanceUseSlot();
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					//TODO: Fix Use slot positioning
					new UseSlot(new Vector2(0.16650043f, 0.3549993f)),
					new UseSlot(new Vector2(0.57749975f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP)
				])
				.Build()),
		];

		protected override bool Persistent => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer &&
						              parameters.Figure.TurnPerformedActionStates.SelectMany(actionState => actionState.AbilityStates)
							              .All(abilityState => abilityState is not MoveAbility.State || !abilityState.Performed),
						async parameters =>
						{
							ActionState actionState = new ActionState(parameters.Figure,
							[
								MoveAbility.Builder().WithDistance(2).Build(),
							]);
							await actionState.Perform();
							await state.AdvanceUseSlot();
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					//TODO: Fix Use slot positioning
					new UseSlot(new Vector2(0.16650043f, 0.3549993f)),
					new UseSlot(new Vector2(0.57749975f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP)
				])
				.Build()),
		];

		protected override bool Persistent => true;
	}
}
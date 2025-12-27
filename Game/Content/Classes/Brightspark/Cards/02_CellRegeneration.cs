using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class CellRegeneration : BrightsparkCardModel<CellRegeneration.CardTop, CellRegeneration.CardBottom>
{
	public override string Name => "Cell Regeneration";
	public override int Level => 1;
	public override int Initiative => 73;
	protected override int AtlasIndex => 2;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(3)
				.WithTarget(Target.Self)
				//TODO: Add Light Consumption
				.Build())
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer,
						async parameters =>
						{
							ActionState actionState = new ActionState(parameters.Figure,
							[
								HealAbility.Builder().WithHealValue(1).Build(),
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
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), FinalSlotAbility)
				])
				.Build())
		];

		protected override bool Persistent => true;

		private async GDTask FinalSlotAbility(AbilityState abilityState)
		{
			ActionState actionState = new ActionState(abilityState.Performer,
			[
				HealAbility.Builder().WithHealValue(1).Build(),
			]);
			await actionState.Perform();
			await AbilityCmd.GainXP(abilityState.Performer, 1);
		}
	}
}
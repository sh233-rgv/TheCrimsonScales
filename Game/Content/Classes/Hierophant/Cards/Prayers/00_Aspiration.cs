using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class Aspiration : HierophantPrayerCardModel<Aspiration.CardTop, Aspiration.CardBottom>
{
	public override string Name => "Aspiration";
	protected override int AtlasIndex => 7 - 0;

	public class CardTop : HierophantPrayerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new UseSlotAbility([new UseSlot(new Vector2(0.398f, 0.314f)), new UseSlot(new Vector2(0.603f, 0.314f))],
				async state =>
				{
					ScenarioEvents.InflictConditionEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Target == state.Performer && canApplyParameters.Condition.IsNegative,
						async applyParameters =>
						{
							if(!applyParameters.Prevented)
							{
								applyParameters.SetPrevented(true);

								ActionState actionState = new ActionState(state.Performer, [new HealAbility(1, target: Target.Self)]);
								await actionState.Perform();

								await state.AdvanceUseSlot();
							}

							await GDTask.CompletedTask;
						});

					await GDTask.CompletedTask;
				},
				async state =>
				{
					ScenarioEvents.InflictConditionEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				}
			))
		];

		protected override bool Persistent => true;
	}

	public class CardBottom : HierophantPrayerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new UseSlotAbility([new UseSlot(new Vector2(0.5f, 0.764f))],
				async state =>
				{
					ScenarioEvents.InflictConditionEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Target == state.Performer && canApplyParameters.Condition.IsNegative,
						async applyParameters =>
						{
							if(!applyParameters.Prevented)
							{
								applyParameters.SetPrevented(true);

								ActionState actionState = new ActionState(state.Performer, [new HealAbility(2, target: Target.Self)]);
								await actionState.Perform();

								await state.AdvanceUseSlot();
							}

							await GDTask.CompletedTask;
						});

					await GDTask.CompletedTask;
				},
				async state =>
				{
					ScenarioEvents.InflictConditionEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				}
			))
		];

		protected override bool Persistent => true;
	}
}
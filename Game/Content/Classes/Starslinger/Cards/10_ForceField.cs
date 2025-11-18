using System.Collections.Generic;
using Godot;
using Fractural.Tasks;

public class ForceField : StarslingerCardModel<ForceField.CardTop, ForceField.CardBottom>
{
	public override string Name => "Force Field";
	public override int Level => 1;
	public override int Initiative => 09;
	protected override int AtlasIndex => 10;

	public class CardTop : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioCheckEvents.ShieldCheckEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Figure.AlliedWith(state.Performer),
						applyParameters =>
						{
							applyParameters.AdjustShield(1);
						}
					);

					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Figure.AlliedWith(state.Performer) && canApplyParameters.FromAttack,
						async applyParameters =>
						{
							applyParameters.AdjustShield(1);

							await state.AdvanceUseSlot();
						}
					);

					AppController.Instance.AudioController.PlayFastForwardable(SFX.Shield, delay: 0f);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
					{
						ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(state, this);

						ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);

						await GDTask.CompletedTask;
					}
				)
				.WithUseSlots(
					[
						new UseSlot(new Vector2(0.16650043f, 0.3549993f), Heal),
						new UseSlot(new Vector2(0.36999783f, 0.3549993f), GainXP),
						new UseSlot(new Vector2(0.57749975f, 0.3549993f), Heal),
						new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP),
						new UseSlot(new Vector2(0.57749975f, 0.3549993f), Heal),
						new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP),
					]
				)
				.Build())
		];

		protected override bool Persistent => true;
		protected override bool Loss => true;

		private async GDTask Heal(AbilityState state)
		{
			ActionState healAbility = new ActionState(state.Performer, [
				HealAbility.Builder()
					.WithHealValue(1)
					.WithRange(1)
					.Build()
			]);
            ScenarioEvents.AbilityEndedEvent.Subscribe(state, this,
				canApplyParameters => true,
				async applyParameters =>
				{
					await healAbility.Perform();
					ScenarioEvents.AbilityEndedEvent.Unsubscribe(state, this);
				});
        }
	}

	public class CardBottom : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Invisible)
				.WithTarget(Target.Self)
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.RoundEndedEvent.Subscribe(state, this,
						parameters => true,
						async parameters =>
						{
							await AbilityCmd.RemoveCondition(state.Performer, Conditions.Invisible);
							
						}
					);
				})
				.WithOnDeactivate(
					async state =>
                    {
						ScenarioEvents.RoundEndedEvent.Unsubscribe(state, this);

						await GDTask.CompletedTask;
                    }
				)
				.WithMandatory(true)
				.Build())
		];

		protected override int XP => 1;
		protected override bool Round => true;
	}
}
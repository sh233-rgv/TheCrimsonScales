using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class ReveredProtector : HierophantLevelUpCardModel<ReveredProtector.CardTop, ReveredProtector.CardBottom>
{
	public override string Name => "Revered Protector";
	public override int Level => 7;
	public override int Initiative => 15;
	protected override int AtlasIndex => 15 - 10;

	public class CardTop : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.FromAttack &&
							canApplyParameters.PotentialAttackAbilityState.Target.AlliedWith(state.Performer, true) &&
							RangeHelper.Distance(canApplyParameters.PotentialAttackAbilityState.Target.Hex, state.Performer.Hex) <= 1,
						async applyParameters =>
						{
							int shieldVal = state.UseSlotIndex switch
							{
								0 => 1,
								1 => 2,
								2 or 3 => 3,
								4 => 4,
								_ => 0
							};
							applyParameters.AdjustShield(shieldVal);
							await state.AdvanceUseSlot();
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
					{
						ScenarioEvents.AfterSufferDamageEvent.Unsubscribe(state, this);

						await GDTask.CompletedTask;
					}
				)
				.WithUseSlots(
					[
						new UseSlot(new Vector2(0.28100282f, 0.3734997f)),
						new UseSlot(new Vector2(0.48650017f, 0.3734997f)),
						new UseSlot(new Vector2(0.68950886f, 0.3734997f)),
						new UseSlot(new Vector2(0.68950886f, 0.3734997f)),
						new UseSlot(new Vector2(0.68950886f, 0.3734997f))
					]
				)
				.Build())
		];

		protected override bool Round => true;
	}

	public class CardBottom : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4)
				.WithMoveType(MoveType.Jump)
				.WithOnAbilityEndedPerformed(async state =>
				{
					foreach(Figure figure in state.Hexes.SelectMany(hex => hex.GetHexObjectsOfType<Figure>().Where(figure => figure.AlliedWith(state.Performer))).Distinct())
					{
						List<ScenarioEvents.GenericChoice.Subscription> subscriptions = [];
						foreach(ConditionModel condition in figure.Conditions)
						{
							subscriptions.Add(ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription.New(
								applyFunction: async parameters =>
								{
									await AbilityCmd.RemoveCondition(figure, condition);
								},
								effectType: EffectType.Selectable,
								effectButtonParameters: new IconEffectButton.Parameters(Icons.GetCondition(condition)),
								effectInfoViewParameters: new TextEffectInfoView.Parameters($"Remove {Icons.Inline(Icons.GetCondition(condition))}")
							));
						}

						await AbilityCmd.GenericChoice(figure, subscriptions);
					}
					await GDTask.CompletedTask;
				})
				.Build()),
		];
	}
}
using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class BefuddlingSerum : BrightsparkCardModel<BefuddlingSerum.CardTop, BefuddlingSerum.CardBottom>
{
	public override string Name => "Befuddling Serum";
	public override int Level => 4;
	public override int Initiative => 49;
	protected override int AtlasIndex => 18;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4)
				.WithRange(2)
				.WithRangeType(RangeType.Melee)
				.WithConditions(Conditions.Muddle)
				.Build())
		];

		protected override int XP => 1;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.DuringAttackEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer,
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAddCondition(Conditions.Muddle);
							await GDTask.CompletedTask;
						});
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer,
						async parameters =>
						{
							switch(state.UseSlotIndex)
							{
								case 0: parameters.AbilityState.SingleTargetAdjustPush(3); break;
								case 1: parameters.AbilityState.SingleTargetAdjustPierce(3); break;
								case 2: parameters.AbilityState.SingleTargetAdjustAttackValue(3); break;
								case 3: parameters.AbilityState.AdjustTargets(1); break;
								default: throw new ArgumentOutOfRangeException();
							}

							await state.AdvanceUseSlot();
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.DuringAttackEvent.Unsubscribe(state, this);
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					//TODO: Fix Use slot positioning
					new UseSlot(new Vector2(0.16650043f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP),
					new UseSlot(new Vector2(0.57749975f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP)
				])
				.Build())
		];

		protected override bool Persistent => true;
		public override bool Loss => true;
	}
}
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class DecayingDaggers : SpiritCallerCardModel<DecayingDaggers.CardTop, DecayingDaggers.CardBottom>
{
	public override string Name => "Decaying Daggers";
	public override int Level => 3;
	public override int Initiative => 28;
	protected override int AtlasIndex => 28 - 15;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithTargets(2)
				.WithRange(3)
				.WithAfterTargetConfirmedSubscription(ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
					parameters =>
						RangeHelper.GetFiguresInRange(parameters.AbilityState.Target, 1, includeNonFigures: true).Any(figure => figure is Spirit),
					async parameters =>
					{
						parameters.AbilityState.SingleTargetAdjustAttackValue(1);

						await GDTask.CompletedTask;
					}))
				.Build())
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						parameters =>
							parameters.FromAttack &&
							parameters.TotalShield > 0 &&
							parameters.PotentialAbilityState.Performer is Spirit spirit &&
							spirit.CharacterOwner == state.Performer,
						async parameters =>
						{
							parameters.AdjustPierce(2);

							await state.AdvanceUseSlot();
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					new UseSlot(new Vector2(0.5f, 0.5f), GainXP),
					new UseSlot(new Vector2(0.5f, 0.5f), Air),
					new UseSlot(new Vector2(0.5f, 0.5f), GainXP),
					new UseSlot(new Vector2(0.5f, 0.5f), Dark),
					new UseSlot(new Vector2(0.5f, 0.5f), GainXP),
				])
				.Build())
		];

		public override bool Persistent => true;
		public override bool Loss => true;

		private async GDTask Air(AbilityState state)
		{
			await AbilityCmd.InfuseElement(state, Element.Air);
		}

		private async GDTask Dark(AbilityState state)
		{
			await AbilityCmd.InfuseElement(state, Element.Dark);
		}
	}
}
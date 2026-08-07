using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class WieldedMemory : IncarnateCardModel<WieldedMemory.CardTop, WieldedMemory.CardBottom>
{
	public override string Name => "Wielded Memory";
	public override int Level => 1;
	public override int Initiative => 38;
	protected override int AtlasIndex => 13;

	public class CardTop : IncarnateCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackDiamond(this, new Vector2(0.6191026f, 0.24412134f)))
				.WithOnAbilityStarted(async state =>
				{
					ScenarioEvents.ItemStateChangedEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Item.Owner == state.Performer &&
						                      canApplyParameters.Item.ItemType is ItemType.OneHand or ItemType.TwoHands,
						async applyParameters =>
						{
							state.AbilityAdjustAttackValue(2);
							await AbilityCmd.GainXP(state.Performer, 1);
							ScenarioEvents.ItemStateChangedEvent.Unsubscribe(state, this);
						}
					);
					await GDTask.CompletedTask;
				})
				.WithOnAbilityEnded(async abilityState =>
					{
						ScenarioEvents.ItemStateChangedEvent.Unsubscribe(abilityState, this);

						await GDTask.CompletedTask;
					}
				)
				.Build())
		];
	}

	public class CardBottom : IncarnateCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(PushAbility.Builder()
				.WithPush(1, new PushCircle(this, new Vector2(0.40413168f, 0.7387121f)))
				.WithTarget(Target.TargetAll | Target.Enemies)
				.WithRange(2)
				.WithConditions(Conditions.Rupture)
				.Build())
		];

		public override int XP => 2;
		public override bool Loss => true;
	}
}
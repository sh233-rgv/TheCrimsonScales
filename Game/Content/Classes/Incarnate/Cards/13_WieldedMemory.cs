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
				.WithDamage(5, new AttackDiamond(this, new Vector2(0.44888887f, 0.23300421f)))
				.WithRange(3, new RangeSquare(this, new Vector2(0.65925926f, 0.23227511f)))
				.WithOnAbilityStarted(async state =>
				{
					ScenarioEvents.ItemStateChangedEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Item.ItemState == ItemState.Consumed && canApplyParameters.Item.Owner == state.Performer,
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

		public override int XP => 2;
		public override bool Round => true;
		public override bool Loss => true;
	}

	public class CardBottom : IncarnateCardSide
	{
		private MoveCircle _moveEnhancementMark;

		protected override void InitExtraEnhancements()
		{
			base.InitExtraEnhancements();

			_moveEnhancementMark = new MoveCircle(this, new Vector2(0.6648147f, 0.7911358f));
		}

		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveCircle(this, new Vector2(0.62007874f, 0.6360111f)))
				.Build()),
			new AbilityCardAbility(GrantAbility.Builder()
				.WithAbilities(MoveAbility.Builder().WithDistance(2, _moveEnhancementMark).Build())
				.WithRange(1)
				.WithConditionalAbilityCheck(state => InSpirit(state, IncarnateSpirit.Conqueror))
				.WithOnAbilityEndedPerformed(async state =>
				{
					await AbilityCmd.InfuseElement(state, Element.Earth);
				})
				.Build()),
		];

		protected override IEnumerable<IncarnateSpirit> SwitchSpiritChoices => [IncarnateSpirit.Ritualist, IncarnateSpirit.Reaver];
	}
}
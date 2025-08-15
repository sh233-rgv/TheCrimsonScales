using System.Collections.Generic;
using Fractural.Tasks;

public class PlayingWithFire : FireKnightCardModel<PlayingWithFire.CardTop, PlayingWithFire.CardBottom>
{
	public override string Name => "Playing With Fire";
	public override int Level => 1;
	public override int Initiative => 40;
	protected override int AtlasIndex => 12 - 5;

	public class CardTop : FireKnightCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new AttackAbility(1, range: 3, conditions: [Conditions.Wound1],
				duringAttackSubscriptions:
				[
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Fire,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAdjustAttackValue(1);
							parameters.AbilityState.AbilitySetHasAdvantage();

							await AbilityCmd.GainXP(parameters.Performer, 1);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Attack)}, advantage")
					)
				]
			)),

			new AbilityCardAbility(GiveFireKnightItemAbility([ModelDB.Item<ExplosiveTonic>(), ModelDB.Item<EmberCladding>()],
				conditionalAbilityCheck: async state =>
				{
					await GDTask.CompletedTask;

					return state.Performer.Hex.HasHexObjectOfType<Ladder>();
				}
			))
		];
	}

	public class CardBottom : FireKnightCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new AttackAbility(6,
				duringAttackSubscriptions:
				[
					ScenarioEvents.DuringAttack.Subscription.New(
						parameters => parameters.Performer.Hex.HasHexObjectOfType<Ladder>(),
						async parameters =>
						{
							parameters.AbilityState.AbilityAdjustRange(2);
							parameters.AbilityState.AbilitySetRangeType(RangeType.Range);

							await GDTask.CompletedTask;
						},
						effectType: EffectType.Selectable,
						canApplyMultipleTimesDuringSubscription: false,
						effectButtonParameters: new IconEffectButton.Parameters(LadderIconPath),
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"+2{Icons.Inline(Icons.Range)}")
					)
				]
			)),
		];

		protected override IEnumerable<Element> Elements => [Element.Fire];
		protected override int XP => 2;
		protected override bool Loss => true;
	}
}
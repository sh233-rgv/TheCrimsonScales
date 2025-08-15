using System.Collections.Generic;
using Fractural.Tasks;

public class FierceLeader : FireKnightCardModel<FierceLeader.CardTop, FierceLeader.CardBottom>
{
	public override string Name => "Fierce Leader";
	public override int Level => 1;
	public override int Initiative => 26;
	protected override int AtlasIndex => 12 - 9;

	public class CardTop : FireKnightCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new GrantAbility(figure => [new AttackAbility(3)], target: Target.Allies)),

			new AbilityCardAbility(GiveFireKnightItemAbility([ModelDB.Item<ScrollOfCharisma>(), ModelDB.Item<ScrollOfInvigoration>()],
				onItemGiven: async (state, item) =>
				{
					await AbilityCmd.GainXP(state.Performer, 1);
				}
			))
		];
	}

	public class CardBottom : FireKnightCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new HealAbility(2, range: 3,
				duringHealSubscriptions:
				[
					ScenarioEvent<ScenarioEvents.DuringHeal.Parameters>.Subscription.ConsumeElement(Element.Fire,
						applyFunction: async applyParameters =>
						{
							applyParameters.AbilityState.SingleTargetAddCondition(Conditions.Strengthen);

							await AbilityCmd.GainXP(applyParameters.Performer, 1);
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Strengthen))}")
					)
				]
			))
		];
	}
}
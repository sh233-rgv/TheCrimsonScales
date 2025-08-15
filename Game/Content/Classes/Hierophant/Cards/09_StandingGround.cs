using System.Collections.Generic;
using Fractural.Tasks;

public class StandingGround : HierophantCardModel<StandingGround.CardTop, StandingGround.CardBottom>
{
	public override string Name => "Standing Ground";
	public override int Level => 1;
	public override int Initiative => 22;
	protected override int AtlasIndex => 29 - 9;

	public class CardTop : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new GrantAbility(figure =>
				[
					new ShieldAbility(2, pierceable: false)
				]
			))
		];

		protected override IEnumerable<Element> Elements => [Element.Earth];
		protected override bool Round => true;
	}

	public class CardBottom : HierophantCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new AttackAbility(2, rangeType: RangeType.Range,
				customGetTargets: (state, list) =>
				{
					foreach(Figure figure in GameController.Instance.Map.Figures)
					{
						foreach(Figure potentialAlly in RangeHelper.GetFiguresInRange(figure.Hex, 1))
						{
							if(state.Performer.AlliedWith(potentialAlly))
							{
								list.Add(figure);
								break;
							}
						}
					}
				}
			)),

			new AbilityCardAbility(new GrantAbility(
				figure =>
				[
					new ShieldAbility(1,
						conditionalAbilityCheck: state => AbilityCmd.AskConsumeElement(state.Performer, Element.Earth),
						onAbilityEndedPerformed: async state =>
						{
							await GDTask.CompletedTask;

							state.ActionState.SetOverrideRound();
						})
				],
				customGetTargets: (state, list) =>
				{
					AttackAbility.State attackAbilityState = state.ActionState.GetAbilityState<AttackAbility.State>(0);

					foreach(Figure target in attackAbilityState.UniqueTargetedFigures)
					{
						list.AddRange(RangeHelper.GetFiguresInRange(target.Hex, 1));
					}
				},
				conditionalAbilityCheck: async state =>
				{
					await GDTask.CompletedTask;

					AttackAbility.State attackAbilityState = state.ActionState.GetAbilityState<AttackAbility.State>(0);

					return attackAbilityState.Performed;
				}
			))
		];
	}
}
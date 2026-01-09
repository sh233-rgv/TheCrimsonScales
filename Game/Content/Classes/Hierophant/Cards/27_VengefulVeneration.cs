using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class VengefulVeneration : HierophantLevelUpCardModel<VengefulVeneration.CardTop, VengefulVeneration.CardBottom>
{
	public override string Name => "Vengeful Veneration";
	public override int Level => 8;
	public override int Initiative => 78;
	protected override int AtlasIndex => 15 - 13;

	public class CardTop : HierophantCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder().WithDamage(4).WithRange(4).Build()),

			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					//TODO: Add visual (character token) to target(?)
					AttackAbility.State attackAbilityState = state.ActionState.GetAbilityState<AttackAbility.State>(0);
					ScenarioEvents.AfterAttackPerformedEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Performer == attackAbilityState.Target &&
							canApplyParameters.AbilityState.Target.AlliedWith(state.Performer),
						async applyParameters =>
						{
							await AbilityCmd.SufferDamage(state, applyParameters.Performer, 2);

							await state.ActionState.RequestDiscardOrLose();
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AfterAttackPerformedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build())
		];

		public override bool Round => true;
	}

	public class CardBottom : HierophantCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3)
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					List<Figure> figures = RangeHelper.GetFiguresInRange(state.Performer.Hex, 1).Where(figure => figure.EnemiesWith(state.Performer))
						.ToList();
					foreach(Figure figure in figures)
					{
						await AbilityCmd.SufferDamage(state, figure, (figures.Count == 1) ? 2 : 1);
						state.SetPerformed();
					}
				})
				.Build())
		];
	}
}
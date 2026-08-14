using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class PerforatingBore : ShardrenderCardModel<PerforatingBore.CardTop, PerforatingBore.CardBottom>
{
	public override string Name => "Perforating Bore";
	public override int Level => 2;
	public override int Initiative => 63;
	protected override int AtlasIndex => 15;

	public class CardTop : ShardrenderCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4)
				.WithPierce(1)
				.WithAfterAttackPerformedSubscription(
					AdvanceCrystallizeSubscription<ScenarioEvents.AfterAttackPerformed.Parameters>(async parameters =>
					{
						Figure figure = await AbilityCmd.SelectFigure(parameters.AbilityState, figures =>
						{
							figures.AddRange(RangeHelper.GetFiguresInRange(parameters.AbilityState.Target, 1, false));
						});
						if(figure != null)
						{
							await AbilityCmd.SufferDamage(parameters.AbilityState, figure, 1);
						}

						await AbilityCmd.GainXP(parameters.Performer, 1);
					}, new TextEffectInfoView.Parameters($"One enemy adjacent to the target suffers {Icons.Inline(Icons.Damage)}1")))
				.Build()),
		];
	}

	public class CardBottom : ShardrenderCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer,
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAdjustPierce(1);
							parameters.AbilityState.SingleTargetAddCondition(Conditions.Brittle);

							await state.ActionState.RequestDiscardOrLose();
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override bool Round => true;
	}
}
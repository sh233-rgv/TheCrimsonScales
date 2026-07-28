using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class CoverOfGreen : ThornreaperCardModel<CoverOfGreen.CardTop, CoverOfGreen.CardBottom>
{
	public override string Name => "Cover of Green";
	public override int Level => 1;
	public override int Initiative => 34;
	protected override int AtlasIndex => 0;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.DuringAttackEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Performer == state.Performer && !canApplyParameters.Performer.IsDamaged(),
						async applyParameters =>
						{
							applyParameters.AbilityState.SingleTargetSetHasAdvantage();

							await GDTask.CompletedTask;
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
					{
						ScenarioEvents.DuringAttackEvent.Unsubscribe(state, this);

						await GDTask.CompletedTask;
					}
				)
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveCircle(this, new Vector2(0.6216662f, 0.6884774f)))
				.Build()),
			new AbilityCardAbility(GrantAbility.Builder()
				.WithAbilities(
				[
					MoveAbility.Builder()
						.WithDistance(1, new MoveCircle(this, new Vector2(0.6216662f, 0.8459244f)))
						.Build()
				])
				.WithRange(3)
				.Build()
			),
		];
	}
}
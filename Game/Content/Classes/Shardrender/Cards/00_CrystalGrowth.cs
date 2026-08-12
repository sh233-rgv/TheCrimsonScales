using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class CrystalGrowth : ShardrenderCardModel<CrystalGrowth.CardTop, CrystalGrowth.CardBottom>
{
	public override string Name => "Aligned Constellations";
	public override int Level => 1;
	public override int Initiative => 68;
	protected override int AtlasIndex => 0;

	public class CardTop : ShardrenderCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CrystallizeAbility.Builder()
				.WithOnActivate(async state =>
				{

				})),

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

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Dark)];
		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ShardrenderCardSide
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
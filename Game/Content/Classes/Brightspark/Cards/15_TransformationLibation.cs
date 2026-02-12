using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class TransformationLibation : BrightsparkCardModel<TransformationLibation.CardTop, TransformationLibation.CardBottom>
{
	public override string Name => "Transformation Libation";
	public override int Level => 2;
	public override int Initiative => 27;
	protected override int AtlasIndex => 15;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					Monster monster = (Monster)await AbilityCmd.SelectFigure(state, list =>
						list.AddRange(RangeHelper.GetFiguresInRange(state.Performer.Hex, 1).Where(figure =>
							figure.EnemiesWith(state.Performer) && figure.Health <= 4 && figure is Monster monster &&
							monster.MonsterType is MonsterType.Normal)));
					if(monster == null)
					{
						state.SetNotPerformed();
						return;
					}

					monster.SetEnemies(Alignment.Enemies);
					monster.SetAlignment(Alignment.Characters);
					ScenarioEvents.FigureTurnStartedEvent.Subscribe(state, this,
						parameters => parameters.Figure == monster,
						async parameters =>
						{
							await AbilityCmd.SufferDamage(monster, 1, monster);
						});
					ScenarioCheckEvents.CanBeTargetedCheckEvent.Subscribe(state, this,
						parameters => parameters.PotentialTarget == monster && parameters.PotentialAbilityState is HealAbility.State,
						parameters =>
						{
							parameters.SetCannotBeTargeted();
						});
					ScenarioEvents.FigureKilledEvent.Subscribe(state, this,
						parameters => parameters.Figure == monster,
						async parameters =>
						{
							await state.ActionState.RequestDiscardOrLose();
						});
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.FigureTurnStartedEvent.Unsubscribe(state, this);
					ScenarioCheckEvents.CanBeTargetedCheckEvent.Unsubscribe(state, this);
					ScenarioEvents.FigureKilledEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Air)];
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ControlAbility.Builder()
				.WithGetAbilities(state =>
					[
						MoveAbility.Builder()
							.WithDistance(2)
							.Build()
					]
				)
				.WithRange(3)
				.Build()),
		];
	}
}
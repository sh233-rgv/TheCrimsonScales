using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class BarbedOnslaught : ThornreaperCardModel<BarbedOnslaught.CardTop, BarbedOnslaught.CardBottom>
{
	public override string Name => "Barbed Onslaught";
	public override int Level => 5;
	public override int Initiative => 58;
	protected override int AtlasIndex => 20;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithCount(2)
				.WithRange(3)
				.WithOnAbilityEndedPerformed(async state =>
				{
					foreach(Figure figure in state.CreatedOverlayTiles.SelectMany(overlayTile => overlayTile.Hex.GetFigures()))
					{
						await AbilityCmd.SufferDamage(state, figure, 2);
					}
				})
				.Build())
		];

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer && parameters.FromAttack &&
						              parameters.Figure.Hex.HasHexObjectOfType<HazardousTerrain>(),
						async parameters =>
						{
							parameters.AdjustShield(1);
							await GDTask.CompletedTask;
						});

					ScenarioEvents.RetaliateEvent.Subscribe(state, this,
						parameters => parameters.RetaliatingFigure == state.Performer &&
						              RangeHelper.Distance(parameters.RetaliatingFigure.Hex, parameters.Performer.Hex) <= 1 &&
						              parameters.RetaliatingFigure.Hex.HasHexObjectOfType<HazardousTerrain>(),
						async parameters =>
						{
							parameters.AdjustRetaliate(2);
							await GDTask.CompletedTask;
						});

					ScenarioCheckEvents.ShieldCheckEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer &&
						              parameters.Figure.Hex.HasHexObjectOfType<HazardousTerrain>(),
						parameters =>
						{
							parameters.AdjustShield(1);
						});

					ScenarioCheckEvents.RetaliateCheckEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer &&
						              parameters.Figure.Hex.HasHexObjectOfType<HazardousTerrain>(),
						parameters =>
						{
							parameters.AddRetaliate(2, 1);
						});

					ScenarioEvents.FigureEnteredHexEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer,
						async _ =>
						{
							ScenarioCheckEvents.ShieldCheckEvent.FireChangedEvent();
							ScenarioCheckEvents.RetaliateCheckEvent.FireChangedEvent();

							await GDTask.CompletedTask;
						}, EffectType.Visuals);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);
					ScenarioEvents.RetaliateEvent.Unsubscribe(state, this);
					ScenarioEvents.FigureEnteredHexEvent.Unsubscribe(state, this);
					ScenarioCheckEvents.ShieldCheckEvent.Unsubscribe(state, this);
					ScenarioCheckEvents.RetaliateCheckEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class EnvironmentalSurvey : BrightsparkCardModel<EnvironmentalSurvey.CardTop, EnvironmentalSurvey.CardBottom>
{
	public override string Name => "Environmental Survey";
	public override int Level => 1;
	public override int Initiative => 31;
	protected override int AtlasIndex => 11;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					Hex hex = (await AbilityCmd.SelectHex(state.Performer, list =>
					{
						list.AddRange(RangeHelper.GetHexesInRange(state.Performer.Hex, 2)
							.Where(hex => hex.TryGetHexObjectOfType(out Obstacle obs) || hex.TryGetHexObjectOfType(out Trap trap)));
					}, hintText: "Select an obstacle to move"));

					if(hex == null)
					{
						return;
					}

					OverlayTile overlayTile;
					if(hex.TryGetHexObjectOfType(out Obstacle obstacle))
					{
						overlayTile = obstacle;
					}
					else
					{
						overlayTile = hex.GetHexObjectOfType<Trap>();
					}

					await AbilityCmd.MoveOverlayTile(state.Performer, overlayTile, list =>
					{
						list.AddRange(RangeHelper.GetHexesInRange(state.Performer.Hex, 2)
							.Where(moveToHex => moveToHex.IsEmpty()));
					});
				})
				.Build())
		];

		protected override IEnumerable<Element> Elements => [Element.Earth];
		protected override int XP => 1;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioCheckEvents.MoveCheckEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Performer.AlliedWith(state.Performer, true) &&
							RangeHelper.Distance(canApplyParameters.Performer.Hex, state.Performer.Hex) <= 4 &&
							(canApplyParameters.Hex.HasHexObjectOfType<DifficultTerrain>() ||
							 canApplyParameters.Hex.HasHexObjectOfType<HazardousTerrain>()),
						applyParameters =>
						{
							if(applyParameters.Hex.HasHexObjectOfType<DifficultTerrain>())
							{
								applyParameters.SetMoveCost(1);
							}

							if(applyParameters.Hex.HasHexObjectOfType<HazardousTerrain>())
							{
								applyParameters.SetAffectedByNegativeHex(false);
							}
						}
					);

					ScenarioEvents.HazardousTerrainTriggeredEvent.Subscribe(state, this,
						canApplyParameters => canApplyParameters.Figure.AlliedWith(state.Performer, true) &&
						                      RangeHelper.Distance(canApplyParameters.Figure.Hex, state.Performer.Hex) <= 4,
						async applyParameters =>
						{
							applyParameters.SetAffectedByHazardousTerrain(false);
							await GDTask.CompletedTask;
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioCheckEvents.MoveCheckEvent.Unsubscribe(state, this);
					ScenarioEvents.HazardousTerrainTriggeredEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.Build())
		];

		protected override int XP => 2;
		protected override bool Persistent => true;
		public override bool Loss => true;
	}
}
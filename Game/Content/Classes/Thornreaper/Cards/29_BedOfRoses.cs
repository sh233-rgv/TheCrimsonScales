using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class BedOfRoses : ThornreaperCardModel<BedOfRoses.CardTop, BedOfRoses.CardBottom>
{
	public override string Name => "Bed of Roses";
	public override int Level => 1;
	public override int Initiative => 10;
	protected override int AtlasIndex => 29;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ShieldAbility.Builder()
				.WithShieldValue(2, new ShieldDiamondPlus(this, new Vector2(0.6169745f, 0.13074793f)))
				.Build()),
			new AbilityCardAbility(RetaliateAbility.Builder()
				.WithRetaliateValue(4, new RetaliateDiamondPlus(this, new Vector2(0.61387014f, 0.2159665f)))
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.RoundEndedEvent.Subscribe(state, this,
						_ => true,
						async _ =>
						{
							List<Hex> hexes = await AbilityCmd.SelectHexes(state, hexes => hexes.Add(state.Performer.Hex), 0, 1, true,
								"Create thorns in the hex you occupy?");
							if(hexes.Count > 0)
							{
								await CreateThorns(state.Performer, state.Performer.Hex);
							}
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.RoundEndedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Light)];
		public override bool Round => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.HazardousTerrainTriggeredEvent.Subscribe(state, this,
						parameters => parameters.Figure.EnemiesWith(state.Performer),
						async parameters =>
						{
							parameters.AddAfterHazardousTerrainDamage(async triggerParameters =>
							{
								if(triggerParameters.Figure.Health <= 5)
								{
									await AbilityCmd.KillOrExhaust(state, triggerParameters.Figure);
									await state.AdvanceUseSlot();
								}
							});

							await GDTask.CompletedTask;
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.HazardousTerrainTriggeredEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					new UseSlot(new Vector2(0.29200172f, 0.74973845f)),
					new UseSlot(new Vector2(0.49901202f, 0.74973845f), GainXP),
					new UseSlot(new Vector2(0.7069984f, 0.74973845f)),
					new UseSlot(new Vector2(0.18780857f, 0.87756234f), GainXP),
					new UseSlot(new Vector2(0.39579493f, 0.87756234f)),
					new UseSlot(new Vector2(0.60455734f, 0.87756234f), GainXP)
				])
				.Build())
		];

		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
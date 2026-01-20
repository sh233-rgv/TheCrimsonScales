using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class WeatherForecast : BrightsparkCardModel<WeatherForecast.CardTop, WeatherForecast.CardBottom>
{
	public override string Name => "Weather Forecast";
	public override int Level => 3;
	public override int Initiative => 30;
	protected override int AtlasIndex => 16;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithRange(3)
				.Build())
		];

		//TODO: public override IEnumerable<Element> Elements => AnyElement;
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => parameters.Figure == state.Performer,
						async parameters =>
						{
							Element element = state.UseSlotIndex switch
							{
								0 => Element.Ice,
								1 => Element.Light,
								2 => Element.Air,
								_ => throw new ArgumentOutOfRangeException()
							};
							await AbilityCmd.InfuseElement(state, element);
							await state.AdvanceUseSlot();
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.WithUseSlots(
				[
					//TODO: Use Slot Positioning
					new UseSlot(new Vector2(0.16650043f, 0.3549993f)),
					new UseSlot(new Vector2(0.57749975f, 0.3549993f)),
					new UseSlot(new Vector2(0.78700954f, 0.3549993f), GainXP)
				])
				.Build()),
		];

		public override bool Persistent => true;
	}
}
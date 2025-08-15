using System.Collections.Generic;
using Fractural.Tasks;

public class Lamentation : HierophantPrayerCardModel<Lamentation.CardTopBottom, Lamentation.CardTopBottom>
{
	public override string Name => "Lamentation";
	protected override int AtlasIndex => 7 - 3;

	public class CardTopBottom : HierophantPrayerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(new OtherActiveAbility(
				async state =>
				{
					ScenarioEvents.SufferDamageEvent.Subscribe(state, this,
						canApplyParameters =>
							canApplyParameters.Figure == state.Performer && canApplyParameters.WouldSufferDamage,
						async applyParameters =>
						{
							applyParameters.SetDamagePrevented();

							await state.ActionState.RequestDiscardOrLose();
						}, EffectType.Selectable);

					await GDTask.CompletedTask;
				},
				async state =>
				{
					ScenarioEvents.SufferDamageEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				}
			))
		];

		protected override bool Persistent => true;
		protected override bool Loss => true;
	}
}
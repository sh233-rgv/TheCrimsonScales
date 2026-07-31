using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class Overgrowth : ThornreaperCardModel<Overgrowth.CardTop, Overgrowth.CardBottom>
{
	public override string Name => "Overgrowth";
	public override int Level => 1;
	public override int Initiative => 92;
	protected override int AtlasIndex => 8;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(0)
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.RoundStartBeforeCardSelectionEvent.Subscribe(state, this,
						_ => true,
						async _ =>
						{
							await AbilityCmd.AddShield(state.Performer, ScenarioEvents.GetSubscriberPair(state, this), 2);

							await AbilityCmd.AddRetaliate(state.Performer, ScenarioEvents.GetSubscriberPair(state, this), 2, 1);

							ScenarioCheckEvents.PotentialTargetCheckEvent.Subscribe(state, this,
								parameters => parameters.PotentialTarget == state.Performer,
								parameters =>
								{
									parameters.SetSortingInitiative(int.MinValue);
								});

							ScenarioEvents.RoundEndedEvent.Subscribe(state, this,
								_ => true,
								async _ =>
								{
									await state.ActionState.RequestDiscardOrLose();
								});
						});

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					AbilityCmd.RemoveRetaliate(state.Performer, ScenarioEvents.GetSubscriberPair(state, this));
					AbilityCmd.RemoveShield(state.Performer, ScenarioEvents.GetSubscriberPair(state, this));
					ScenarioEvents.RoundStartBeforeCardSelectionEvent.Unsubscribe(state, this);
					ScenarioEvents.RoundEndedEvent.Unsubscribe(state, this);
					ScenarioCheckEvents.PotentialTargetCheckEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Earth)),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3, new MoveSquare(this, new Vector2(0.5235183f, 0.8127503f)))
				.WithMoveType(MoveType.Jump)
				.Build())
		];
	}
}
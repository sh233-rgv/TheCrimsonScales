using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class UnholySacrifice : SpiritCallerCardModel<UnholySacrifice.CardTop, UnholySacrifice.CardBottom>
{
	public override string Name => "Unholy Sacrifice";
	public override int Level => 1;
	public override int Initiative => 23;
	protected override int AtlasIndex => 28 - 12;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4)
				.WithTargets(1)
				.WithRange(3)
				.WithOnAbilityStarted(async state =>
				{
					state.AdjustTargets(state.Performer is Character characterOwner ? Spirit.GetSpirits(characterOwner).Count : 0);

					await GDTask.CompletedTask;
				})
				.Build()),
		];

		public override int XP => 2;
		public override bool Loss => true;
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3)
				.Build()),

			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.HexObjectDestroyedEvent.Subscribe(state, this,
						parameters => parameters.HexObject is Spirit,
						async parameters =>
						{
							await state.AdvanceUseSlot();

							ActionState actionState = new ActionState(state.ActionState, state.Performer,
							[
								PullAbility.Builder()
									.WithPull(1)
									.WithRange(2)
									.WithCustomGetPerformHex(pullState => parameters.HexObject.Hex)
									.Build()
							]);
							await actionState.Perform();
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.HexObjectDestroyedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithUseSlot(new UseSlot(new Vector2(0.5f, 0.9f))) //TODO
				.Build())
		];

		public override bool Persistent => true;
	}
}
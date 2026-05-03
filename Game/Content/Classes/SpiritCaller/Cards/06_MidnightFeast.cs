using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class MidnightFeast : SpiritCallerCardModel<MidnightFeast.CardTop, MidnightFeast.CardBottom>
{
	public override string Name => "Midnight Feast";
	public override int Level => 1;
	public override int Initiative => 80;
	protected override int AtlasIndex => 28 - 6;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(new DynamicInt<HealAbility.State>(state =>
					3 + (state.Performer is Character characterOwner ? Spirit.GetSpirits(characterOwner).Count : 0)))
				.WithRange(3)
				.Build()),
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(UseSlotAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.HexObjectDestroyedEvent.Subscribe(state, this,
						parameters => parameters.HexObject is Spirit,
						async parameters =>
						{
							await state.AdvanceUseSlot();

							foreach(Figure figure in RangeHelper.GetFiguresInRange(parameters.HexObject.Hex, 1, true))
							{
								if(state.Performer.EnemiesWith(figure))
								{
									await AbilityCmd.SufferDamage(state, figure, 2);
									await AbilityCmd.AddCondition(state, figure, Conditions.Curse);
								}
							}

							ActionState actionState = new ActionState(state.ActionState, state.Performer,
							[
								HealAbility.Builder()
									.WithHealValue(3)
									.WithCustomGetTargets((healState, list) =>
									{
										foreach(Figure figure in RangeHelper.GetFiguresInRange(parameters.HexObject.Hex, 1, true))
										{
											if(healState.Performer.AlliedWith(figure))
											{
												list.Add(figure);
											}
										}
									})
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

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
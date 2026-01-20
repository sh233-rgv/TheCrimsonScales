using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class AcquireFunding : BrightsparkCardModel<AcquireFunding.CardTop, AcquireFunding.CardBottom>
{
	public override string Name => "Acquire Funding";
	public override int Level => 1;
	public override int Initiative => 61;
	protected override int AtlasIndex => 0;

	public class CardTop : BrightsparkCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithRange(3)
				.WithOnAbilityStarted(async abilityState =>
				{
					bool lootCoin = false;
					ScenarioCheckEvents.SpawnCoinCheckEvent.Subscribe(abilityState, this,
						canApplyParameters => abilityState.Target == canApplyParameters.Figure && canApplyParameters.SpawnCoin,
						applyParameters =>
						{
							applyParameters.SetSpawnCoin(false);
							lootCoin = true;
						}, order: 100
					);
					ScenarioEvents.AfterAttackPerformedEvent.Subscribe(abilityState, this,
						parameters => parameters.AbilityState.Target.IsDead && lootCoin,
						async parameters =>
						{
							PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Scenario/CoinStack.tscn");
							CoinStack coinStack = scene.Instantiate<CoinStack>();
							GameController.Instance.Map.AddChild(coinStack);
							await coinStack.Init(abilityState.Target.Hex);

							await coinStack.Loot(abilityState.Performer);
						});

					await GDTask.CompletedTask;
				})
				.WithOnAbilityEnded(async abilityState =>
					{
						ScenarioCheckEvents.SpawnCoinCheckEvent.Unsubscribe(abilityState, this);
						ScenarioEvents.AfterAttackPerformedEvent.Unsubscribe(abilityState, this);

						await GDTask.CompletedTask;
					}
				)
				.Build())
		];
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
							ActionState actionState = new ActionState(parameters.Figure,
							[
								MoveAbility.Builder().WithDistance(1).Build(),
								LootAbility.Builder().WithRange(1).Build()
							]);
							await actionState.Perform();
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
					new UseSlot(new Vector2(0.29199997f, 0.7944986f)),
					new UseSlot(new Vector2(0.4999998f, 0.7944986f)),
					new UseSlot(new Vector2(0.7079987f, 0.7944986f), GainXP)
				])
				.Build())
		];

		public override bool Persistent => true;
		public override bool Loss => true;
	}
}
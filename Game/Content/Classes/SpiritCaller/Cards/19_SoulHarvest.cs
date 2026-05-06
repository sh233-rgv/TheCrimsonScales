using System.Collections.Generic;
using Fractural.Tasks;

public class SoulHarvest : SpiritCallerCardModel<SoulHarvest.CardTop, SoulHarvest.CardBottom>
{
	public override string Name => "Soul Harvest";
	public override int Level => 5;
	public override int Initiative => 15;
	protected override int AtlasIndex => 28 - 19;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AbilityStartedEvent.Subscribe(state, this,
						parameters =>
							parameters.AbilityState is AttackAbility.State &&
							parameters.Performer is Spirit,
						async parameters =>
						{
							((AttackAbility.State)parameters.AbilityState).AbilitySetHasAdvantage();

							await GDTask.CompletedTask;
						}
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.AbilityStartedEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.Build()),

			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					Spirit spirit = await Spirit.SelectSpirit(state);

					if(spirit != null)
					{
						await spirit.RemoveDamageCounters(1);
						state.SetPerformed();
					}
				})
				.Build())
		];

		public override bool Round => true;
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SpawnAbility.Builder()
				.WithName("Shifting Discs")
				.WithTexturePath("res://Content/Classes/SpiritCaller/Summons/orb_of_light.png")
				.WithHealth(2)
				.WithMove(2)
				.Build()),

			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					bool canUse = true;

					ScenarioEvents.FigureTurnEndedEvent.Subscribe(state, this,
						parameters => true,
						async parameters =>
						{
							canUse = true;

							await GDTask.CompletedTask;
						}
					);

					SpawnAbility.State spawnAbilityState = state.ActionState.GetAbilityState<SpawnAbility.State>(0);

					AbilityCmd.SubscribeDuringCharacterTurn(ScenarioEvents.GetSubscriberPair(state, this), EffectType.Selectable,
						character =>
							canUse &&
							AbilityCmd.CanSwap(spawnAbilityState.Spirit, character),
						async character =>
						{
							canUse = false;
							await AbilityCmd.TrySwap(state, spawnAbilityState.Spirit, character);
						},
						new IconEffectButton.Parameters(Icons.Teleport),
						new TextEffectInfoView.Parameters($"Swap hexes with Shifting Discs.")
					);

					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					// ScenarioEvents.FigureEnteredHexEvent.Unsubscribe(state, this);

					await GDTask.CompletedTask;
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.WithMandatory(true)
				.WithSkipConfirmation()
				.Build())
		];

		public override int XP => 1;
		public override bool Persistent => true;
	}
}
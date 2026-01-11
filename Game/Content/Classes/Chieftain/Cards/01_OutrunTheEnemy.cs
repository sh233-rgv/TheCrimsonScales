using System.Collections.Generic;
using Fractural.Tasks;

public class OutrunTheEnemy : ChieftainCardModel<OutrunTheEnemy.CardTop, OutrunTheEnemy.CardBottom>
{
	public override string Name => "Outrun the Enemy";
	public override int Level => 1;
	public override int Initiative => 87;
	protected override int AtlasIndex => 1;

	public class CardTop : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithName("Speedy Ostrich")
				.WithTexturePath("res://Content/Classes/Chieftain/Summons/speedy_ostrich_AI.png")
				.WithHealth(4)
				.WithMove(3)
				.WithAttack(1)
				.WithTraits(new MountTrait(async (owner, mount) =>
					{
						ScenarioEvents.RoundStartedBeforeInitiativesSortedEvent.Subscribe(owner, this,
							canApplyParameters => true,
							async applyParameters =>
							{
								ScenarioCheckEvents.InitiativeCheckEvent.Subscribe(owner, this,
									parameters => parameters.Figure == owner,
									parameters => parameters.AdjustInitiative(-10)
								);

								owner.UpdateInitiative();
								ScenarioCheckEvents.InitiativeCheckEvent.Unsubscribe(owner, this);

								await GDTask.CompletedTask;
							});

						await GDTask.CompletedTask;
					},
					async (owner, mount) =>
					{
						ScenarioEvents.RoundStartedBeforeInitiativesSortedEvent.Unsubscribe(owner, this);

						await GDTask.CompletedTask;
					}))
				.Build()
			),
		];

		public override int XP => 2;
		public override bool Persistent => true;
		public override bool Loss => true;
	}

	public class CardBottom : ChieftainCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(GrantAbility.Builder()
				.WithAbilities(
				[
					MoveAbility.Builder()
						.WithDistance(0)
						.WithOnAbilityStarted(async moveState =>
						{
							moveState.AdjustMoveValue(((Summon)moveState.Performer).Stats.Move ?? 0);

							await GDTask.CompletedTask;
						})
						.WithDuringMovementSubscription(
							ScenarioEvents.DuringMovement.Subscription.ConsumeElement(Element.Earth,
								applyFunction: async applyParameters =>
								{
									applyParameters.AbilityState.AdjustMoveValue(2);

									await AbilityCmd.GainXP(applyParameters.Performer, 1);
								},
								effectInfoViewParameters: new TextEffectInfoView.Parameters($"+2{Icons.Inline(Icons.Move)}")
							)
						)
						.Build()
				])
				.WithCustomGetTargets((grantState, figures) =>
				{
					figures.AddRange(((Character)grantState.Performer).Summons);
				})
				.WithTarget(Target.Allies)
				.Build()
			),
		];
	}
}
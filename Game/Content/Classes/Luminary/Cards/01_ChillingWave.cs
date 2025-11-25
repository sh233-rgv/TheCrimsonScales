using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class ChillingWave : LuminaryCardModel<ChillingWave.CardTop, ChillingWave.CardBottom>
{
	public override string Name => "Chilling Wave";
	public override int Level => 1;
	public override int Initiative => 39;
	protected override int AtlasIndex => 1;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Empty),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Empty),
					]
				))
				.WithDuringAttackSubscription(
					ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Dark,
						applyFunction: async parameters =>
						{
							parameters.AbilityState.AbilityAddCondition(Conditions.Stun);

							await GDTask.CompletedTask;
						},
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Stun))}")
					)
				)
				.Build()),
			Scuttle(1, [Element.Ice]),
		];
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AbilityStartedEvent.Subscribe(state, this,
						canApply: parameters => parameters.AbilityState.GetCustomValue<bool>("Glow", "Glow Ability"),
						apply: async parameters =>
                        {
                            if (parameters.AbilityState is TargetedAbilityState targetedAbilityState)
                            {
                                targetedAbilityState.AbilityAddCondition(Conditions.Stun);
                            }
							await state.ActionState.RequestDiscardOrLose();
							//TODO: Add Remove Immediately
                        });
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
					foreach(AbilityCard abilityCard in ((Character)state.Performer).Cards)
					{
						AbilityState glowState =
							abilityCard.ActiveActionStates
								.SelectMany(a => a.AbilityStates)
								.FirstOrDefault(s => s.GetCustomValue<bool>("Glow", "Active Glow"));
						if (glowState != null)
						{
							Ability ability = glowState.GetCustomValue<Ability>("Glow", "Glow Perform");
							ActionState actionState = new(state.Performer,[ability]);
							await actionState.Perform();
							state.SetPerformed();
							break;
						}
					}
					await GDTask.CompletedTask;
				})
				.Build())
		];

		protected override int XP => 2;
		protected override bool Round => true;
		protected override bool Loss => true;
	}
}
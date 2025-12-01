using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class ShadowClaws : LuminaryCardModel<ShadowClaws.CardTop, ShadowClaws.CardBottom>
{
	public override string Name => "Shadow Claws";
	public override int Level => 5;
	public override int Initiative => 25;
	protected override int AtlasIndex => 21;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow(new GlowAbilityModel([Element.Dark], GlowAbility,
				$"Perform {Icons.Inline(Icons.GetCondition(Conditions.Muddle))} ability", Icons.GetCondition(Conditions.Muddle)))
		];
		
		protected override int XP => 1;
		protected override bool Persistent => true;

		protected Ability GlowAbility(List<Element> elements)
        {
			return ConditionAbility.Builder()
				.WithConditions(Conditions.Muddle)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
					]
				))
				.WithOnAbilityStarted(async state =>
				{
					state.SetCustomValue(state.Performer, "Glow Ability", true);
					state.SetCustomValue(state.Performer, "Consumed Elements", elements);

					await GDTask.CompletedTask;
				})
				.WithOnAbilityEnded(async state =>
                {
					if (ScenarioEvents.FindSubscriberPair(state.Performer, this) == null)
					{
						ScenarioEvents.AttackAfterTargetConfirmedEvent.Subscribe(state.Performer, this,
							parameters => parameters.AbilityState.Target.HasCondition(Conditions.Muddle),
							async parameters =>
							{
								parameters.AbilityState.SingleTargetSetHasAdvantage();

								await GDTask.CompletedTask;
							}
						);

						ScenarioEvents.FigureTurnEndedEvent.Subscribe(state.Performer, this,
							parameters => true,
							async parameters =>
							{
								ScenarioEvents.AttackAfterTargetConfirmedEvent.Unsubscribe(state.Performer, this);
								ScenarioEvents.FigureTurnEndedEvent.Unsubscribe(state.Performer, this);

								await GDTask.CompletedTask;
							}
						);
					}
                    
					await GDTask.CompletedTask;
                })
				.Build();
        }
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					List<ScenarioEvents.GenericChoice.Subscription> subscriptions = [];
					foreach(ConditionModel condition in state.Performer.Conditions)
					{
						subscriptions.Add(ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription.New(
							applyFunction: async parameters =>
							{
								await AbilityCmd.RemoveCondition(state.Performer, condition);
								state.SetPerformed();
							},
							effectType: EffectType.Selectable,
							effectButtonParameters: new IconEffectButton.Parameters(Icons.GetCondition(condition)),
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"Remove {Icons.Inline(Icons.GetCondition(condition))}")
						));
					}

					await AbilityCmd.GenericChoice(state.Authority, subscriptions);
				})
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
                {
                    ScenarioEvents.AbilityEndedEvent.Subscribe(state, this,
						canApply: parameters => parameters.AbilityState.Performer == state.Performer &&
									parameters.AbilityState.GetCustomValue<bool>(state.Performer, "Glow Ability") &&
									parameters.AbilityState.TryGetCustomValue<List<Element>>(state.Performer, "Consumed Elements", out _),
						async parameters =>
						{
							await state.ActionState.RequestDiscardOrLose();
							//TODO: Discard immediately
							await AbilityCmd.InfuseElement(global::Elements.All
								.Except(parameters.AbilityState.GetCustomValue<List<Element>>(state.Performer, "Consumed Elements"))
								.ToList(), state.Authority, state);
						},
						effectButtonParameters: new IconEffectButton.Parameters(Icons.GetAnyElement()),
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"Infuse {Icons.Inline(Icons.GetAnyElement())} other than any of the consumed elements"),
						effectType: EffectType.Selectable
					);
					await GDTask.CompletedTask;
                })
				.WithOnDeactivate(async state =>
                {
                    ScenarioEvents.AbilityEndedEvent.Unsubscribe(state, this);
                    await GDTask.CompletedTask;
                })
				.Build()),
		];

		protected override bool Persistent => true;
	}
}
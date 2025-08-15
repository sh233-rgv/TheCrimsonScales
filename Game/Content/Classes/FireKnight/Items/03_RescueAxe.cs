using Fractural.Tasks;

public class RescueAxe : FireKnightItem
{
	public override string Name => "Rescue Axe";
	public override int ItemNumber => 3;
	protected override int AtlasIndex => 10 - 3;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					await AbilityCmd.InfuseElement(Element.Fire);

					ActionState actionState = new ActionState(user,
						[
							new AttackAbility(2, conditions: [Conditions.Muddle],
								duringAttackSubscriptions:
								[
									ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Fire,
										applyFunction: async parameters =>
										{
											parameters.AbilityState.AbilityAddCondition(Conditions.Wound1);
											parameters.AbilityState.AbilityRemoveCondition(Conditions.Muddle);

											await GDTask.CompletedTask;
										},
										effectInfoViewParameters: new TextEffectInfoView.Parameters($"{Icons.Inline(Icons.GetCondition(Conditions.Wound1))} instead")
									)
								]
							)
						]
					);
					await actionState.Perform();
				});
			}
		);
	}
}
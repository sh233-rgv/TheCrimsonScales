using Fractural.Tasks;

public class ExplosiveTonic : FireKnightItem
{
	public override string Name => "Explosive Tonic";
	public override int ItemNumber => 7;
	protected override int AtlasIndex => 10 - 7;

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
							new AttackAbility(1, range: 3, conditions: [Conditions.Wound1],
								duringAttackSubscriptions:
								[
									ScenarioEvents.DuringAttack.Subscription.ConsumeElement(Element.Fire,
										applyFunction: async parameters =>
										{
											parameters.AbilityState.AbilityAdjustAttackValue(1);

											await GDTask.CompletedTask;
										},
										effectInfoViewParameters: new TextEffectInfoView.Parameters($"+1{Icons.Inline(Icons.Attack)}")
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
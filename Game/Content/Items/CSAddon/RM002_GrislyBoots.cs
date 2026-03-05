using Fractural.Tasks;

public class GrislyBoots : CSAddonRM
{
	public override string Name => "Grisly Boots";
	public override int ItemNumber => 2;
	public override int ShopCount => 1;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Feet;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 13;

	private object _subscriber;

	public override void Init(Character owner)
	{
		_subscriber = new object();

		base.Init(owner);
	}

	protected override void Subscribe()
	{
		base.Subscribe();

		ScenarioEvents.AbilityPerformedEvent.Subscribe(this, _subscriber,
			parameters => parameters.AbilityState is MoveAbility.State && parameters.AbilityState.Performer == Owner,
			async parameters =>
			{
				await Use(async user =>
				{
					ActionState actionState = new ActionState(user,
					[
						PushAbility.Builder()
							.WithPush(1)
							.WithRange(1)
							.WithAbilityStartedSubscription(
								ScenarioEvents.AbilityStarted.Subscription.New(
									pushParameters => pushParameters.Performer.RoundMovedHexes.Count >= 4,
									async pushParameters =>
									{
										((PushAbility.State)pushParameters.AbilityState).AbilityAdjustPush(1);
										await GDTask.CompletedTask;
									}))
					]);
					await actionState.Perform();
				});
			});
	}

	protected override void Unsubscribe()
	{
		base.Unsubscribe();

		ScenarioEvents.AbilityPerformedEvent.Unsubscribe(this, _subscriber);
	}
}
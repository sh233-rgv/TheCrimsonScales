using Fractural.Tasks;

public class FueledFalchion : GHRewardsItem
{
	public override string Name => "Fueled Falchion";
	public override int ItemNumber => 116;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 23;

	private object _subscriber;

	public override void Init(Character owner)
	{
		_subscriber = new object();

		base.Init(owner);
	}

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state =>
				state.Performer == Owner &&
				state.SingleTargetRangeType == RangeType.Melee &&
				state.AbilityTargets == 1,
			apply: async state =>
			{
				await Use(async user =>
				{
					ScenarioEvents.AfterAttackPerformedEvent.Subscribe(this, _subscriber,
						parameters => parameters.AbilityState == state,
						async parameters =>
						{
							ScenarioEvents.AfterAttackPerformedEvent.Unsubscribe(this, _subscriber);

							foreach(Figure figure in RangeHelper.GetFiguresInRange(parameters.AbilityState.Target.Hex, 1, requiresLineOfSight: false))
							{
								await AbilityCmd.SufferDamage(null, figure, 1);
							}
						}
					);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
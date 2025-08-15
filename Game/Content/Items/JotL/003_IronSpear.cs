using Fractural.Tasks;

public class IronSpear : JotLItem
{
	public override string Name => "Iron Spear";
	public override int ItemNumber => 3;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 2;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner && state.SingleTargetRangeType == RangeType.Melee && state.AbilityTargets == 1 && state.SingleTargetRange == 1,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAdjustRange(1);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
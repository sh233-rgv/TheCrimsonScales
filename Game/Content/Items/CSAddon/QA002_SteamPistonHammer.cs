using Fractural.Tasks;

public class SteamPistonHammer : CSAddonQA
{
	public override string Name => "Steam Piston Hammer";
	public override int ItemNumber => 2;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 4;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner && state.SingleTargetRangeType == RangeType.Melee,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAddCondition(Conditions.Muddle);
					state.SingleTargetAdjustPush(1);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
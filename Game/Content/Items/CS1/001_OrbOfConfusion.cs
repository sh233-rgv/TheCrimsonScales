using Fractural.Tasks;

public class OrbOfConfusion : CS1Item
{
	public override string Name => "Orb of Confusion";
	public override int ItemNumber => 1;
	public override int ShopCount => 2;
	public override int Cost => 15;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override int MaxUseCount => 3;

	protected override int AtlasIndex => 0;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAddCondition(Conditions.Muddle);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
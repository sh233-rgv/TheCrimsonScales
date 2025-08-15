using Fractural.Tasks;

public class DizzyingTincture : CS2Item
{
	public override string Name => "Dizzying Tincture";
	public override int ItemNumber => 58;
	public override int ShopCount => 2;
	public override int Cost => 15;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 32;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAddCondition(Conditions.Disarm);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
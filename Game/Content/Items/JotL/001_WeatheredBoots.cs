using Fractural.Tasks;

public class WeatheredBoots : JotLItem
{
	public override string Name => "Weathered Boots";
	public override int ItemNumber => 1;
	public override int ShopCount => 2;
	public override int Cost => 15;
	public override ItemType ItemType => ItemType.Feet;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 0;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringMove(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.AdjustMoveValue(1);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}
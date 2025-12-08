public class AshsteelGauntlets : CS2Item
{
	public override string Name => "Ashsteel Gauntlets";
	public override int ItemNumber => 61;
	public override int ShopCount => 1;
	public override int Cost => 60;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 34;

	protected override void Subscribe()
	{

		//TODO: Code actual ability
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					
				});
			}
		);
	}
}
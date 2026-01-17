public class LongSpear : Prosperity3Item
{
	public override string Name => "Long Spear";
	public override int ItemNumber => 26;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.TwoHands;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 8;

	protected override void Subscribe()
	{
		base.Subscribe();
	}
}
public class BootsOfStriding : Prosperity1Item
{
	public override string Name => "Boots of Striding";
	public override int ItemNumber => 1;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Feet;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 0;
}
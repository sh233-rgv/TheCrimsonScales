public class EagleEyeGoggles : Prosperity1Item
{
	public override string Name => "Eagle-Eye Goggles";
	public override int ItemNumber => 6;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.Head;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 10;
}
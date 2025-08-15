public class WarHammer : Prosperity1Item
{
	public override string Name => "War Hammer";
	public override int ItemNumber => 10;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.TwoHands;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 18;
}
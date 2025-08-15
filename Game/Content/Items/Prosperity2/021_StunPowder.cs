public class StunPowder : Prosperity2Item
{
	public override string Name => "Stun Powder";
	public override int ItemNumber => 21;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 14;
}
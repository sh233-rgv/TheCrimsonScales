public class WeightedNet : Prosperity2Item
{
	public override string Name => "Weighted Net";
	public override int ItemNumber => 19;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.TwoHands;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 8;
}
public class EmpoweringTalisman : Prosperity2Item
{
	public override string Name => "Empowering Talisman";
	public override int ItemNumber => 17;
	public override int ShopCount => 2;
	public override int Cost => 45;
	public override ItemType ItemType => ItemType.Head;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 4;
}
public class CloakOfInvisibility : Prosperity1Item
{
	public override string Name => "Cloak of Invisibility";
	public override int ItemNumber => 5;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Body;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 8;
}
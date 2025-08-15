public class MinorStaminaPotion : Prosperity1Item
{
	public override string Name => "Minor Stamina Potion";
	public override int ItemNumber => 13;
	public override int ShopCount => 2;
	public override int Cost => 10;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 26;
}
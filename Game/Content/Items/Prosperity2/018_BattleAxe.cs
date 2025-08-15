public class BattleAxe : Prosperity2Item
{
	public override string Name => "Battle-Axe";
	public override int ItemNumber => 18;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 6;
}
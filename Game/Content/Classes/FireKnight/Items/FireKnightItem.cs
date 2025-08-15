public abstract class FireKnightItem : AtlasItemModel
{
	public override string ItemGroupId => "FK";

	protected override int ColumnCount => 6;
	protected override int RowCount => 2;
	protected override string TexturePath => "res://Content/Classes/FireKnight/Items.jpg";

	public override int ShopCount => 0;
	public override int Cost => 0;

	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
}
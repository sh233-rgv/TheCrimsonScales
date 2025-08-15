public abstract class JotLItem : AtlasItemModel
{
	public override string ItemGroupId => "JotL";

	protected override int ColumnCount => 5;
	protected override int RowCount => 2;
	protected override string TexturePath => "res://Content/Items/JotL/SpriteSheet.jpg";
}
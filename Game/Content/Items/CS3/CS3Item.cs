public abstract class CS3Item : AtlasItemModel
{
	public override string ItemGroupId => "CS";

	protected override int ColumnCount => 6;
	protected override int RowCount => 2;
	protected override string TexturePath => "res://Content/Items/CS3/SpriteSheet.jpg";
}
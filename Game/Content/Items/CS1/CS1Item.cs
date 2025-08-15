public abstract class CS1Item : AtlasItemModel
{
	public override string ItemGroupId => "CS";

	protected override int ColumnCount => 10;
	protected override int RowCount => 5;
	protected override string TexturePath => "res://Content/Items/CS1/SpriteSheet.jpg";
}
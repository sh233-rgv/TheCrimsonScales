public abstract class CS4Item : AtlasItemModel
{
	public override string ItemGroupId => "CS";

	protected override int ColumnCount => 9;
	protected override int RowCount => 2;
	protected override string TexturePath => "res://Content/Items/CS4/SpriteSheet.jpg";
}
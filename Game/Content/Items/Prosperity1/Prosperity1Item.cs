public abstract class Prosperity1Item : AtlasItemModel
{
	public override string ItemGroupId => "GH";

	protected override int ColumnCount => 9;
	protected override int RowCount => 4;
	protected override string TexturePath => "res://Content/Items/Prosperity1/SpriteSheet.jpg";
}
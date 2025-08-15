public abstract class Prosperity2Item : AtlasItemModel
{
	public override string ItemGroupId => "GH";

	protected override int ColumnCount => 6;
	protected override int RowCount => 3;
	protected override string TexturePath => "res://Content/Items/Prosperity2/SpriteSheet.jpg";
}
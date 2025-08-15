public abstract class GHDesignsItem : AtlasItemModel
{
	public override string ItemGroupId => "GH";

	protected override int ColumnCount => 6;
	protected override int RowCount => 5;
	protected override string TexturePath => "res://Content/Items/GHDesigns/SpriteSheet.jpg";
}
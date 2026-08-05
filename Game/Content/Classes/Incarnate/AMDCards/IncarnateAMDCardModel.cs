public abstract class IncarnateAMDCardModel : AMDCardModel
{
	protected override string GetTexturePath(AMDCardOwner owner) => "res://Content/Classes/Ruinmaw/AMDCards.jpg";
	protected override int ColumnCount => 3;
	protected override int RowCount => 2;
}
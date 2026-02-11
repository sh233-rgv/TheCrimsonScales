public abstract class BombardAMDCardModel : AMDCardModel
{
	protected override string GetTexturePath(AMDCardOwner owner) => "";
	protected override int ColumnCount { get; }
	protected override int RowCount { get; }
}
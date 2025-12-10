public abstract class SanctuaryCritAMDCardModel : AMDCardModel
{
	protected override string TexturePath => "res://Art/AMDs/SanctuaryCrit.jpg";
	protected override int ColumnCount => 5;
	protected override int RowCount => 2;

	public override bool RemoveAfterDraw { get; protected set; } = false;

	public override AMDCardType Type => AMDCardType.Crit;
}
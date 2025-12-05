public class CritAMDCard : DefaultAMDCardModel
{
	protected override int AtlasIndex => 18;

	public override bool Reshuffles => true;
	public override AMDCardType Type => AMDCardType.Crit;
}
public class BlessAMDCard : DefaultOtherAMDCardModel
{
	protected override int AtlasIndex => 3;

	public override bool Reshuffles => true;
	public override bool RemoveAfterDraw { get; protected set; } = false;
	public override AMDCardType Type => AMDCardType.Crit;
}
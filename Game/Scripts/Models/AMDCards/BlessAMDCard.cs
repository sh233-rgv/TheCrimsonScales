public class BlessAMDCard : DefaultOtherAMDCardModel
{
	public override string GetSimpleString(RichTextParameters richTextParameters) =>
		GetSimpleString(richTextParameters, AMDCardType.Crit, petals: "Bless");

	public override string ToString(RichTextParameters richTextParameters) =>
		GetBasicString(richTextParameters, AMDCardType.Crit, petals: "Bless");

	protected override int AtlasIndex => 3;
	public override bool RemoveAfterDraw => true;
	public override AMDCardType Type => AMDCardType.Crit;
}
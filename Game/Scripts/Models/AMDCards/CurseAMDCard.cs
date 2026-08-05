public class CurseAMDCard : DefaultOtherAMDCardModel
{
	public override string GetSimpleString(RichTextParameters richTextParameters) =>
		GetSimpleString(richTextParameters, AMDCardType.Null, petals: "Curse");

	public override string ToString(RichTextParameters richTextParameters) =>
		GetBasicString(richTextParameters, AMDCardType.Null, petals: "Curse");

	protected override int AtlasIndex => 2;
	public override bool RemoveAfterDraw => true;
	public override AMDCardType Type => AMDCardType.Null;
}
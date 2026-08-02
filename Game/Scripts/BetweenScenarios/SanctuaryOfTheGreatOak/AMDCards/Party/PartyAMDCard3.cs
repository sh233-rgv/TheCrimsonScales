public class PartyAMDCard3 : PartyAMDCardModel
{
	public override string GetSimpleString(RichTextParameters richTextParameters) =>
		GetSimpleString(richTextParameters, +0,
			$"{Icons.Inline(Icons.Targets, richTextParameters)}{Icons.Inline(Icons.Rolling, richTextParameters)}");

	public override string ToString(RichTextParameters richTextParameters) =>
		GetBasicString(richTextParameters, +0,
			extraText: $"+1{Icons.Inline(Icons.Targets, richTextParameters)} adjacent to this target",
			rolling: true);

	protected override int AtlasIndex => 8;

	//TODO: Actually implement this effect
	public override int? AddedTargets => 1;
}
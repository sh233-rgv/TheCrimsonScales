public class RuinmawEmpowerAMDCard : EmpowerAMDCard
{
	public override string GetSimpleString(RichTextParameters richTextParameters) =>
		GetSimpleString(richTextParameters, +1, $"{Icons.Inline(Icons.Push, richTextParameters)}1{Icons.Inline(Icons.Rolling, richTextParameters)}");

	public override string ToString(RichTextParameters richTextParameters) =>
		GetBasicString(richTextParameters, +1, rolling: true, petals: "Empower");

	protected override string GetTexturePath(AMDCardOwner owner) => "res://Content/Classes/Ruinmaw/AMDCards/AMDCards.png";
	protected override int ColumnCount => 3;
	protected override int RowCount => 2;
	protected override int AtlasIndex => 5;
	public override int? GetValue(AttackAbility.State attackAbilityState) => +1;
	public override bool GetRolling(AttackAbility.State attackAbilityState) => true;
	public override int? Push => 1;
}
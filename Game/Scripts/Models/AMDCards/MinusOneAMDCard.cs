public class MinusOneAMDCard : DefaultAMDCardModel
{
	protected override int AtlasIndex => 11;

	public override int? GetValue(AttackAbility.State state) => -1;
}
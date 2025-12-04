public class PlusZeroAMDCard : DefaultAMDCardModel
{
	protected override int AtlasIndex => 0;

	public override int? GetValue(AttackAbility.State state) => +0;
}
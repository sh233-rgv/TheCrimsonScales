public class PushSanctuaryRollingAMDCard : SanctuaryCritAMDCardModel
{
	protected override int AtlasIndex => 0;

	public override int? GetValue(AttackAbility.State attackAbilityState) => 1;

	public override int? Push => 2;
}
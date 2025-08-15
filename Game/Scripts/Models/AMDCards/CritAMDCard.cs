public class CritAMDCard : AMDCard
{
	public override bool Reshuffles => true;
	public override bool IsCrit => true;

	public CritAMDCard(string textureAtlasPath, int atlasIndex, int textureAtlasColumnCount, int textureAtlasRowsCount)
		: base(textureAtlasPath, atlasIndex, textureAtlasColumnCount, textureAtlasRowsCount)
	{
	}

	protected override int GetValue(AttackAbility.State attackAbilityState)
	{
		return attackAbilityState.SingleTargetAttackValue;
	}

	public override (int, bool) GetScore(AttackAbility.State attackAbilityState)
	{
		return (attackAbilityState.SingleTargetAttackValue, false);
	}
}
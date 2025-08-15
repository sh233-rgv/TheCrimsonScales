public class NullAMDCard : AMDCard
{
	public override bool Reshuffles => true;
	public override bool IsNull => true;

	public NullAMDCard(string textureAtlasPath, int atlasIndex, int textureAtlasColumnCount, int textureAtlasRowsCount)
		: base(textureAtlasPath, atlasIndex, textureAtlasColumnCount, textureAtlasRowsCount)
	{
	}

	protected override int GetValue(AttackAbility.State attackAbilityState)
	{
		return -attackAbilityState.SingleTargetAttackValue;
	}

	public override (int, bool) GetScore(AttackAbility.State attackAbilityState)
	{
		return (-attackAbilityState.SingleTargetAttackValue, false);
	}
}
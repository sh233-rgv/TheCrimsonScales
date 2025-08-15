public class BlessAMDCard : AMDCard
{
	public override bool RemoveAfterDraw => true;
	public override bool IsCrit => true;

	public BlessAMDCard(string textureAtlasPath, int atlasIndex, int textureAtlasColumnCount, int textureAtlasRowsCount)
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
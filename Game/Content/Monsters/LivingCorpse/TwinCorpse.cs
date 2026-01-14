using System.Linq;

public class TwinCorpse : LivingCorpse
{
	public override MonsterStats[] EliteLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * (CharacterCount - 1)
			})
			.ToArray();

	public override string Name => "Twin Corpse";

	public override int MaxStandeeCount => 2;
}
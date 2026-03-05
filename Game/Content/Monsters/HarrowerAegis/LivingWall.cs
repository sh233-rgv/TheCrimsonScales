using System.Linq;

public class LivingWall : HarrowerAegis
{
	public override MonsterStats[] NamedLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * CharacterCount / 2,
			})
			.ToArray();

	public override string Name => "Living Wall";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<HarrowerAegis>();
}
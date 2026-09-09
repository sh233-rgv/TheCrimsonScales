public class LostRanger : InoxArcher
{
	private readonly MonsterStats _stats =
		new MonsterStats()
		{
			Health = 10,
			Attack = 3,
			Range = 5
		};

	public override MonsterStats[] NamedLevelStats => [_stats, _stats, _stats, _stats, _stats, _stats, _stats, _stats];

	public override string Name => "Lost Ranger";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<InoxArcher>();

	//TODO: Look at card back
}
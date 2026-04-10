public class KillSpecificEnemiesTypeGoal : ScenarioGoal
{
	private readonly MonsterModel _monsterModel;
	private readonly bool _multiple;

	public override string Text => _multiple ? $"Kill all {_monsterModel.Name} enemies." : $"Kill the {_monsterModel.Name}.";

	public KillSpecificEnemiesTypeGoal(MonsterModel monsterModel, bool multiple = false, int order = 1)
		: base(order)
	{
		_monsterModel = monsterModel;
		_multiple = multiple;
	}
}
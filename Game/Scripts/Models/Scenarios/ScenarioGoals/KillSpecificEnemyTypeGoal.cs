using Fractural.Tasks;

public class KillSpecificEnemyTypeGoal : ScenarioGoal
{
	private readonly MonsterModel _monsterModel;
	private readonly bool _multiple;

	public override string Text => _multiple ? $"Kill all {_monsterModel.Name} enemies." : $"Kill the {_monsterModel.Name}.";

	public KillSpecificEnemyTypeGoal(MonsterModel monsterModel, bool multiple = false, int order = 1)
		: base(order)
	{
		_monsterModel = monsterModel;
		_multiple = multiple;
	}

	public override async GDTask Start()
	{
		await base.Start();

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters => SpecificEnemyRemaining(),
			async parameters =>
			{
				await Complete();
			}
		);
	}

	public bool SpecificEnemyRemaining()
	{
		string monsterModelId = _monsterModel.Id.ToString();
		foreach(MonsterSpawner monsterSpawner in GameController.Instance.Map.GetChildrenOfType<MonsterSpawner>())
		{
			if(monsterSpawner.MonsterModelId == monsterModelId)
			{
				// Monster of this type still needs to be spawned
				return false;
			}
		}

		foreach(Figure figure in GameController.Instance.Map.Figures)
		{
			if(figure is Monster monster && monster.MonsterModel == _monsterModel)
			{
				// Monster of this type is still alive
				return false;
			}
		}

		return true;
	}
}
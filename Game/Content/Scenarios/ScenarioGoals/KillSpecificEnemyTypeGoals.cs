public class KillSpecificEnemyTypeGoals : ScenarioGoals
{
	private readonly MonsterModel _monsterModel;

	public override string Text { get; }

	public KillSpecificEnemyTypeGoals(MonsterModel monsterModel, string text)
	{
		_monsterModel = monsterModel;
		Text = text;
	}

	public override void Start()
	{
		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters =>
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
			},
			async parameters =>
			{
				await Win();
			});
	}
}
public class KillAllEnemiesScenarioGoals(bool enemiesToBeSpawned = false) : ScenarioGoals
{
	public override string Text => "Kill all enemies to win this scenario.";
	public bool EnemiesToBeSpawned = enemiesToBeSpawned;

	public override void Start()
	{
		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters =>
			{
				if(EnemiesToBeSpawned)
				{
					return false;
				}

				return EnemiesRemaining();
			},
			async parameters =>
			{
				await Win();
			}
		);
	}

	public static bool EnemiesRemaining()
	{
		foreach(Room room in GameController.Instance.Map.Rooms)
		{
			if(!room.Revealed)
			{
				return false;
			}
		}

		foreach(Figure figure in GameController.Instance.Map.Figures)
		{
			if(figure.Alignment == Alignment.Enemies)
			{
				return false;
			}
		}

		return true;
	}
}
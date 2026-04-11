using System.Linq;
using Fractural.Tasks;

public class KillAllEnemiesScenarioGoal : ScenarioGoal
{
	private bool _enemiesToBeSpawned;

	public override string Text => "Kill all enemies.";

	public KillAllEnemiesScenarioGoal(bool enemiesToBeSpawned = false, int order = 0)
		: base(order)
	{
		_enemiesToBeSpawned = enemiesToBeSpawned;
	}

	public override async GDTask Start()
	{
		await base.Start();

		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters => !_enemiesToBeSpawned && NoEnemiesRemaining(),
			async parameters =>
			{
				await Complete();
			}
		);
	}

	public void DisableEnemiesToBeSpawned()
	{
		_enemiesToBeSpawned = false;
	}

	public static bool NoEnemiesRemaining(bool countObjectives = true, bool revealedOnly = false)
	{
		if(!revealedOnly && GameController.Instance.Map.Rooms.Any(room => !room.Revealed))
		{
			return false;
		}

		foreach(Figure figure in GameController.Instance.Map.Figures)
		{
			if(figure.Alignment == Alignment.Enemies && (figure is not Objective || countObjectives))
			{
				return false;
			}
		}

		return true;
	}
}
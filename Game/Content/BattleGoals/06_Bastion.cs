using System.Linq;
using Fractural.Tasks;

public class Bastion : TheCrimsonScalesBattleGoal
{
	public override string Title => "Bastion";
	public override string Description => "At the end of any round, be adjacent to at least two monsters while standing on a door hex.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		ScenarioEvents.RoundEndedEvent.Subscribe(this,
			parameters =>
				GameController.Instance.Map.Doors.Any(door => door.Hex == character.Hex) &&
				RangeHelper.GetFiguresInRange(character.Hex, 1, false, false).Count(figure => character.EnemiesWith(figure)) >= 2,
			async parameters =>
			{
				battleGoal.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);

		await GDTask.CompletedTask;
	}
}
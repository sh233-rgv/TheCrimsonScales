using System.Linq;
using Fractural.Tasks;

public class Aggressor : TheCrimsonScalesBattleGoal
{
	public override string Title => "Aggressor";
	public override string Description => "Have one or more monsters present on the map at the beginning of every round during the scenario.";

	public override BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.Two;

	public override bool FailIfProgressFull => true;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		ScenarioEvents.RoundStartBeforeCardSelectionEvent.Subscribe(this,
			parameters =>
				!GameController.Instance.Map.Figures.Any(figure => figure is Monster),
			async parameters =>
			{
				battleGoal.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);

		await GDTask.CompletedTask;
	}
}
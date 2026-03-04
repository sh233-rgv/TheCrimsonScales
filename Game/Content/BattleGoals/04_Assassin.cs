using System.Collections.Generic;
using Fractural.Tasks;

public class Assassin : TheCrimsonScalesBattleGoal
{
	public override string Title => "Assassin";
	public override string Description => "Kill a monster before it performs any actions in the scenario.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		List<Monster> actionPerformers = new List<Monster>();

		ScenarioEvents.ActionStartedEvent.Subscribe(this,
			parameters => parameters.ActionState.Performer is Monster,
			async parameters =>
			{
				actionPerformers.AddIfNew((Monster)parameters.ActionState.Performer);

				await GDTask.CompletedTask;
			}
		);

		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters =>
				parameters.PotentialKiller == character &&
				parameters.Figure is Monster monster &&
				!actionPerformers.Contains(monster),
			async parameters =>
			{
				battleGoal.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);

		await GDTask.CompletedTask;
	}
}
using Fractural.Tasks;

public class Prohibitionist : TheCrimsonScalesBattleGoal
{
	public override string Title => "Prohibitionist";
	public override string Description => "Never use a potion.";

	public override bool FailIfProgressFull => true;

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		ScenarioEvents.ItemUseStartedEvent.Subscribe(this,
			parameters =>
				!battleGoal.ProgressFull &&
				parameters.Performer == character && parameters.Item is IPotion,
			async parameters =>
			{
				battleGoal.AdjustProgress(1);

				await GDTask.CompletedTask;
			});

		await GDTask.CompletedTask;
	}
}
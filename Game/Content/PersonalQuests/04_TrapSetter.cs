using Fractural.Tasks;

public class TrapSetter : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Trap Setter";
	public override ClassModel ClassToUnlock => ModelDB.Class<ChainguardModel>();
	public override int MaxProgress => 15;
	protected override int AtlasIndex => 4;

	public override async GDTask OnScenarioSetupPhaseCompleted(Figure figure, PersonalQuestData personalQuestData)
	{
		await base.OnScenarioSetupPhaseCompleted(figure, personalQuestData);

		// Cause enemy to trigger a trap
		ScenarioEvents.TrapTriggeredEvent.Subscribe(figure, this,
			parameters =>
				GameController.Instance.Map.CurrentTurnTaker == figure &&
				figure.EnemiesWith(parameters.Figure) &&
				parameters.PotentialAbilityState?.Authority == figure,
			async parameters =>
			{
				personalQuestData.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);

		// Disarm a trap
		ScenarioEvents.TrapDisarmedEvent.Subscribe(figure, this,
			parameters =>
				GameController.Instance.Map.CurrentTurnTaker == figure &&
				parameters.PotentialDisarmer == figure,
			async parameters =>
			{
				personalQuestData.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);
	}
}
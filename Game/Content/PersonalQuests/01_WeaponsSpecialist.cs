using Fractural.Tasks;

public class WeaponsSpecialist : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Weapons Specialist";
	public override ClassModel ClassToUnlock => ModelDB.Class<BombardModel>();
	public override int MaxProgress => 15;
	protected override int AtlasIndex => 1;

	public override async GDTask OnScenarioSetupPhaseCompleted(Figure figure, PersonalQuestData personalQuestData)
	{
		await base.OnScenarioSetupPhaseCompleted(figure, personalQuestData);

		ScenarioEvents.ItemStateChangedEvent.Subscribe(figure, this,
			parameters =>
				parameters.Item.Owner == figure &&
				parameters.Item.ItemType is ItemType.OneHand or ItemType.TwoHands &&
				parameters.Item.ItemState == ItemState.Consumed,
			async parameters =>
			{
				personalQuestData.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);
	}
}
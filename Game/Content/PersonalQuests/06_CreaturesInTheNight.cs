using Fractural.Tasks;

public class CreaturesInTheNight : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Creatures in the Night";
	public override ClassModel ClassToUnlock => ModelDB.Class<ChieftainModel>();
	public override int MaxProgress => 20;
	protected override int AtlasIndex => 6;

	public override async GDTask OnScenarioSetupPhaseCompleted(Figure figure, PersonalQuestData personalQuestData)
	{
		await base.OnScenarioSetupPhaseCompleted(figure, personalQuestData);

		ScenarioEvents.FigureKilledEvent.Subscribe(figure, this,
			parameters =>
				parameters.PotentialKiller == figure &&
				parameters.Figure is Monster monster &&
				(monster.MonsterModel == ModelDB.Monster<Ooze>() ||
				 monster.MonsterModel == ModelDB.Monster<BloodOoze>() ||
				 monster.MonsterModel == ModelDB.Monster<ForestImp>() ||
				 monster.MonsterModel == ModelDB.Monster<BlackImp>()),
			async parameters =>
			{
				personalQuestData.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);
	}
}
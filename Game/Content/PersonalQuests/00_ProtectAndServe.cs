using Fractural.Tasks;

public class ProtectAndServe : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Protect and Serve";
	public override ClassModel ClassToUnlock => ModelDB.Class<BombardModel>();
	public override int MaxProgress => 10;

// 	public override ScenarioModel UnlockedScenarioModel => ModelDB.Scenario<Scenario033>();
	protected override int AtlasIndex => 0;

	public override async GDTask OnScenarioSetupPhaseCompleted(Figure figure, PersonalQuestData personalQuestData)
	{
		await base.OnScenarioSetupPhaseCompleted(figure, personalQuestData);

		ScenarioEvents.FigureKilledEvent.Subscribe(figure, this,
			parameters =>
				parameters.PotentialKiller == figure &&
				parameters.Figure is Monster monster &&
				(monster.MonsterModel == ModelDB.Monster<InoxGuard>() ||
				 monster.MonsterModel == ModelDB.Monster<InoxArcher>() ||
				 monster.MonsterModel == ModelDB.Monster<InoxShaman>()),
			async parameters =>
			{
				personalQuestData.AdjustProgress(1);

				await GDTask.CompletedTask;
			}
		);
	}
}
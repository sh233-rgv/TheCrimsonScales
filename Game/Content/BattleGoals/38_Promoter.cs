using Fractural.Tasks;

public class Promoter : TheCrimsonScalesBattleGoal
{
	public override string Title => "Promoter";
	public override string Description => "Perform an ability targeting an ally before your first rest and in between each of your rests.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
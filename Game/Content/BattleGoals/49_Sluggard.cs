using Fractural.Tasks;

public class Sluggard : TheCrimsonScalesBattleGoal
{
	public override string Title => "Sluggard";
	public override string Description => "Perform a long rest while at your maximum hit point value, after you have already suffered damage.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO

		await GDTask.CompletedTask;
	}
}
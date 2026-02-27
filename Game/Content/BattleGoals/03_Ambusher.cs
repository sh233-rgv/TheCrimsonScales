using Fractural.Tasks;

public class Ambusher : TheCrimsonScalesBattleGoal
{
	public override string Title => "Ambusher";
	public override string Description => "Open a door and end your move ability adjacent to a monster in the revealed room.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
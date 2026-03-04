using Fractural.Tasks;

public class Ambusher : TheCrimsonScalesBattleGoal
{
	public override string Title => "Ambusher";
	public override string Description => "Open a door and end your move ability adjacent to an enemy in the revealed room.";

	public override async GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoal)
	{
		ScenarioEvents.RoomRevealedEvent.Subscribe(this,
			parameters =>
				parameters.OpenedDoor != null &&
				parameters.PotentialOpener == character,
			async parameters =>
			{
			}
		);

		//TODO: Implement
		await GDTask.CompletedTask;
	}
}
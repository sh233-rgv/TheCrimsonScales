using Fractural.Tasks;
using Godot;

public abstract class EventReward
{
	public abstract EventRewardType Type { get; }
	public abstract string GetLabelText(Color textColor);

	public virtual async GDTask ImmediateResolve()
	{
		await GDTask.CompletedTask;
	}

	// public virtual async GDTask OnAfterFirstRoomRevealed()
	// {
	// 	await GDTask.CompletedTask;
	// }

	public virtual async GDTask OnScenarioSetupPhaseCompleted()
	{
		await GDTask.CompletedTask;
	}
}
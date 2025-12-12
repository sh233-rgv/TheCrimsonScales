using Fractural.Tasks;

public abstract class EventReward
{
	public abstract EventRewardType Type { get; }
	public abstract string LabelText { get; }

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
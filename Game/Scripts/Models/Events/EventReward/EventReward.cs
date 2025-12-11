using Fractural.Tasks;

public abstract class EventReward
{
	public abstract EventRewardType Type { get; }
	public abstract string LabelText { get; }

	public virtual async GDTask Resolve()
	{
		await GDTask.CompletedTask;
	}
}
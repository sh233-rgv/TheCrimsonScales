using Fractural.Tasks;

public abstract class ScenarioGoal : IEventSubscriber
{
	public abstract string Text { get; }
	public int Order { get; }

	public bool Completed { get; private set; }
	public bool Failed { get; private set; }

	public ScenarioGoal(int order)
	{
		Order = order;
	}

	public virtual async GDTask Start()
	{
		await GDTask.CompletedTask;
	}

	protected async GDTask Complete()
	{
		Completed = true;

		await GDTask.CompletedTask;
	}

	protected async GDTask Fail()
	{
		Failed = true;

		await AbilityCmd.Lose();
	}
}
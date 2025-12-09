using Fractural.Tasks;

public abstract class EventChoiceModel
{
	public abstract string Text { get; }

	public EventResolveType GetEventResolveType(EventState state) => EventResolveType.Lost;

	public virtual void Resolve(EventState state)
	{
	}

	public virtual async GDTask ScenarioStart(EventState state)
	{
		await GDTask.CompletedTask;
	}
}
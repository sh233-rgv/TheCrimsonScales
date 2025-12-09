using Fractural.Tasks;

public abstract class EventChoiceModel
{
	public abstract string ChoiceText { get; }

	public abstract string GetStoryText(EventState state);

	public EventResolveType GetEventResolveType(EventState state) => EventResolveType.Lost;

	public virtual async GDTask Resolve(EventState state, SavedCampaign savedCampaign)
	{
		await GDTask.CompletedTask;
	}

	public virtual async GDTask ScenarioStart(EventState state)
	{
		await GDTask.CompletedTask;
	}
}
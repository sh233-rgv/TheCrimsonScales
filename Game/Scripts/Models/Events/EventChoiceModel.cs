using Fractural.Tasks;

public abstract class EventChoiceModel : AbstractModel<EventChoiceModel>
{
	public abstract string ChoiceText { get; }

	public abstract string GetStoryText(SavedEventState state);

	public EventResolveType GetEventResolveType(SavedEventState state) => EventResolveType.Lost;

	public virtual async GDTask Resolve(SavedEventState state, SavedCampaign savedCampaign)
	{
		await GDTask.CompletedTask;
	}

	public virtual async GDTask ScenarioStart(SavedEventState state)
	{
		await GDTask.CompletedTask;
	}
}
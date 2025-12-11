using System;
using System.Collections.Generic;
using Fractural.Tasks;

public abstract class EventChoiceModel : AbstractModel<EventChoiceModel>
{
	public abstract string ChoiceText { get; }

	public virtual void InitState(SavedEventState state, SavedCampaign savedCampaign)
	{
	}

	public virtual EventResolveType GetEventResolveType(SavedEventState state) => EventResolveType.Lost;

	public abstract string GetStoryText(SavedEventState state);

	public abstract List<EventReward> GetRewards(SavedEventState state);
}
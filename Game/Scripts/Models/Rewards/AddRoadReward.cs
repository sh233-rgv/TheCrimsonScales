using Fractural.Tasks;
using Godot;

public class AddRoadReward(EventModel eventModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Road Event {eventModel.Number} is added to the Road Event deck.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

		savedCampaign.SavedEvents.AddRoadEventToDeck(eventModel, BetweenScenariosController.Instance.RNG);
	}
}
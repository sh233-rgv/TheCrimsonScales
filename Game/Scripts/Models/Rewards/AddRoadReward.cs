using System.Threading;
using Fractural.Tasks;

public class AddRoadReward(EventModel eventModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => $"Road Event {eventModel.Number} is added to the Road Event deck.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		savedCampaign.SavedEvents.AddRoadEventToDeck(eventModel, BetweenScenariosController.Instance.RNG);
	}
}
using System.Threading;
using Fractural.Tasks;
using Godot;

public class AddCityReward(EventModel eventModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => $"City Event {eventModel.Number} is added to the City Event deck.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		savedCampaign.SavedEvents.AddCityEventToDeck(eventModel, BetweenScenariosController.Instance.RNG);
	}
}
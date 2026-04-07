using Fractural.Tasks;
using Godot;

public class AddCityReward(EventModel eventModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"City Event {eventModel.Number} is added to the City Event deck.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		BetweenScenariosController.Instance.SavedCampaign.SavedEvents.AddCityEventToDeck(eventModel, BetweenScenariosController.Instance.RNG);
	}
}
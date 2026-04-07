using Fractural.Tasks;
using Godot;

public class LoseProsperityReward(int prosperity) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Lose {prosperity} prosperity.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		BetweenScenariosController.Instance.SavedCampaign.AdjustProsperity(-prosperity);
	}
}
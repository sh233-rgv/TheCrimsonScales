using Fractural.Tasks;
using Godot;

public class UnlockPartyAMDReward(AMDCardModel cardModel) : Reward
{
	public override RewardType Type => RewardType.Immediate;

	public override string GetLabelText(RichTextParameters parameters) =>
		$"Unlocked a bonus card whenever a character makes a donation to the Sanctuary of the Great Oak.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		BetweenScenariosController.Instance.SavedCampaign.SanctuaryOfTheGreatOak.UnlockPartyAMD(cardModel);
	}
}
using Fractural.Tasks;
using Godot;

public class LoseCheckmarkReward() : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Lose 1 {Icons.Inline(Icons.Checkmark, parameters)} each.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		foreach(SavedCharacter character in BetweenScenariosController.Instance.SavedCampaign.Characters)
		{
			character.RemoveCheckmark();
		}
	}
}
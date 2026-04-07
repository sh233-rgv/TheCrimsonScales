using Fractural.Tasks;
using Godot;

public class LoseCheckmarkReward() : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Lose 1 {Icons.Inline(Icons.Checkmark, parameters)} each.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

		foreach(SavedCharacter character in savedCampaign.Characters)
		{
			character.RemoveCheckmark();
		}
	}
}
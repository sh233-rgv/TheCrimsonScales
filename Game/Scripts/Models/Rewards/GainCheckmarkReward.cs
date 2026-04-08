using System.Threading;
using Fractural.Tasks;
using Godot;

public class GainCheckmarkReward() : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => $"Gain 1 {Icons.Inline(Icons.Checkmark, textParameters)} each.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		foreach(SavedCharacter character in savedCampaign.Characters)
		{
			character.AddCheckmark();
		}
	}
}
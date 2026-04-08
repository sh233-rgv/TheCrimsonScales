using System.Threading;
using Fractural.Tasks;
using Godot;

public class GainXPReward(int xp) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => $"Gain {Icons.Inline(Icons.XP, textParameters)}{xp} each.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await base.ImmediateResolve(savedCampaign, cancellationToken);

		foreach(SavedCharacter savedCharacter in savedCampaign.Characters)
		{
			savedCharacter.AddXP(xp);
		}
	}
}
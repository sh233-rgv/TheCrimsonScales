using Fractural.Tasks;
using Godot;

public class GainXPReward(int xp) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain {Icons.Inline(Icons.XP, parameters)}{xp} each.";

	public override async GDTask ImmediateResolve(SavedCampaign savedCampaign)
	{
		await base.ImmediateResolve(savedCampaign);

		foreach(SavedCharacter savedCharacter in savedCampaign.Characters)
		{
			savedCharacter.AddXP(xp);
		}
	}
}
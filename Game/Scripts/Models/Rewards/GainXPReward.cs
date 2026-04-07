using Fractural.Tasks;
using Godot;

public class GainXPReward(int xp) : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain {Icons.Inline(Icons.XP, parameters)}{xp} each.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		foreach(SavedCharacter savedCharacter in BetweenScenariosController.Instance.SavedCampaign.Characters)
		{
			savedCharacter.AddXP(xp);
		}
	}
}
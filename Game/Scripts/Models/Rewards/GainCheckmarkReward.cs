using Fractural.Tasks;
using Godot;

public class GainCheckmarkReward() : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters parameters) => $"Gain 1 {Icons.Inline(Icons.Checkmark, parameters)} each.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		foreach(SavedCharacter character in BetweenScenariosController.Instance.SavedCampaign.Characters)
		{
			character.AddCheckmark();
		}
	}
}
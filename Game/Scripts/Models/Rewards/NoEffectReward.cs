using Godot;

public class NoEffectReward() : Reward
{
	public override RewardType Type => RewardType.Immediate;
	public override string GetLabelText(RichTextParameters textParameters) => "No effect.";
}
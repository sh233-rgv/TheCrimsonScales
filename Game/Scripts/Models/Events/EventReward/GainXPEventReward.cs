using Fractural.Tasks;
using Godot;

public class GainXPEventReward(int xp) : EventReward
{
	public override EventRewardType Type => EventRewardType.Immediate;
	public override string GetLabelText(Color textColor) => $"Gain {xp} {Icons.Inline(Icons.XP, color: textColor)} each.";

	public override async GDTask ImmediateResolve()
	{
		await base.ImmediateResolve();

		//TODO: Adjust reputation
	}
}
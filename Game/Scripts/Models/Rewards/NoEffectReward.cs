using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class NoEffectReward : Reward
{
	public override RewardType Type => RewardType.Immediate;

	public NoEffectReward()
	{
	}

	public override string GetLabelText(RichTextParameters textParameters) => "No effect.";
}
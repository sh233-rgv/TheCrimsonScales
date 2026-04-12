// using System;
// using Fractural.Tasks;
// using Newtonsoft.Json;
//
// [Serializable, JsonObject(MemberSerialization.OptIn)]
// public class OnScenarioStartedReward(Func<GDTask> onScenarioSetupPhaseCompleted, Func<RichTextParameters, string> labelText) : Reward
// {
// 	public override RewardType Type => RewardType.ScenarioStart;
// 	public override string GetLabelText(RichTextParameters textParameters) => labelText(textParameters);
//
// 	public override async GDTask OnScenarioSetupPhaseCompleted()
// 	{
// 		await base.OnScenarioSetupPhaseCompleted();
//
// 		if(onScenarioSetupPhaseCompleted != null)
// 		{
// 			await onScenarioSetupPhaseCompleted();
// 		}
// 	}
// }


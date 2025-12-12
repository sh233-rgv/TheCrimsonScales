using System;
using Fractural.Tasks;

public class OnScenarioStartedEventReward(Func<GDTask> onScenarioSetupPhaseCompleted, string labelText) : EventReward
{
	public override EventRewardType Type => EventRewardType.ScenarioStart;
	public override string LabelText => labelText;

	public override async GDTask OnScenarioSetupPhaseCompleted()
	{
		await base.OnScenarioSetupPhaseCompleted();

		if(onScenarioSetupPhaseCompleted != null)
		{
			await onScenarioSetupPhaseCompleted();
		}
	}
}
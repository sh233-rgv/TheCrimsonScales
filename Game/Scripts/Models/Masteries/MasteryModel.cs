using System;
using Fractural.Tasks;

public abstract class MasteryModel : AbstractModel, IEventSubscriber
{
	public abstract string Description { get; }

	public virtual bool SingleScenario => true;
	public virtual int MaxProgress => 1;

	public virtual async GDTask OnScenarioSetupPhaseCompleted(Mastery mastery, Character character)
	{
		await GDTask.CompletedTask;
	}

	public virtual async GDTask OnScenarioCompleted(Mastery mastery, Character character)
	{
		if(mastery.TemporarilyCompleted && !mastery.TemporarilyFailed)
		{
			mastery.Completed = true;
			character.SavedCharacter.AddAvailablePerk();
		}
		if(SingleScenario)
		{
			await mastery.AdjustProgress(-mastery.Progress);
			await mastery.SetTemporarilyFailed(false);
		}
	}
}
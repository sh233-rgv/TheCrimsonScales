using System;
using Fractural.Tasks;

public abstract class MasteryModel : AbstractModel, IEventSubscriber
{
	public abstract string Description { get; }
	public int Progress { get; private set; }
	public bool Failed { get; private set; }
	public bool TemporarilyCompleted { get; private set; }
	public bool Completed { get; private set; }

	public virtual bool SingleScenario => true;
	public virtual int MaxProgress => 1;

	public event Action ProgressChangedEvent;

	public virtual async GDTask OnScenarioSetupPhaseCompleted(Character character)
	{
		await GDTask.CompletedTask;
	}

	public virtual async GDTask OnScenarioCompleted(Character character)
	{
		if(TemporarilyCompleted)
		{
			Completed = true;
			character.SavedCharacter.AddAvailablePerk();
		}
		if(SingleScenario)
		{
			await AdjustProgress(-Progress);
			await SetFailed(false);
		}
	}

	public async GDTask AdjustProgress(int num = 1)
	{
		if(!Failed)
		{
			Progress += Math.Max(MaxProgress, Progress + num);
			if(Progress == MaxProgress)
			{
				TemporarilyCompleted = true;
			}

			ProgressChangedEvent?.Invoke();
		}

		await GDTask.CompletedTask;
	}

	public async GDTask SetFailed(bool failed)
	{
		Failed = failed;
		ProgressChangedEvent?.Invoke();
		await GDTask.CompletedTask;
	}
}
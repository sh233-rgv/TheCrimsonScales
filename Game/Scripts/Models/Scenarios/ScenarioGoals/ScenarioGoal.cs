using System;
using Fractural.Tasks;

public abstract class ScenarioGoal : IEventSubscriber
{
	public int Order { get; }

	public bool Completed { get; private set; }
	public bool Failed { get; private set; }

	public int Progress { get; private set; }
	public int? MaxProgress { get; protected set; }

	public virtual bool HasProgress => true;

	protected bool FullProgressCompletes => true;

	public event Action<ScenarioGoal> ProgressUpdatedEvent;

	protected ScenarioGoal(int order)
	{
		Order = order;
	}

	public abstract string GetLabelText(RichTextParameters textParameters);

	public virtual async GDTask Start()
	{
		await GDTask.CompletedTask;
	}

	protected async GDTask Complete()
	{
		if(Completed)
		{
			return;
		}

		Completed = true;

		await GDTask.CompletedTask;
	}

	protected async GDTask Fail()
	{
		Failed = true;

		await AbilityCmd.Lose();
	}

	protected async GDTask AdjustProgress(int amount)
	{
		await SetProgress(Progress + amount);
	}

	protected async GDTask SetProgress(int progress)
	{
		Progress = progress;

		if(FullProgressCompletes && MaxProgress.HasValue && Progress >= MaxProgress)
		{
			await Complete();
		}

		ProgressUpdatedEvent?.Invoke(this);
	}

	protected async GDTask SetMaxProgress(int? maxProgress)
	{
		MaxProgress = maxProgress;

		if(FullProgressCompletes && MaxProgress.HasValue && Progress >= MaxProgress)
		{
			await Complete();
		}

		ProgressUpdatedEvent?.Invoke(this);
	}
}
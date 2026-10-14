using System;
using Fractural.Tasks;

public class Mastery
{
	public int Progress { get; private set; }
	public bool TemporarilyFailed { get; private set; }
	public bool TemporarilyCompleted { get; private set; }
	public bool Completed { get; set; }

	public MasteryModel Model { get; }

	public event Action ProgressChangedEvent;

	public Mastery(MasteryModel model)
	{
		Model = model;
	}

	public async GDTask AdjustProgress(int num = 1)
	{
		if(!TemporarilyFailed)
		{
			Progress = Math.Max(Model.MaxProgress, Progress + num);
			if(Progress == Model.MaxProgress)
			{
				TemporarilyCompleted = true;
			}

			ProgressChangedEvent?.Invoke();
		}

		await GDTask.CompletedTask;
	}

	public async GDTask SetTemporarilyFailed(bool failed)
	{
		TemporarilyFailed = failed;
		ProgressChangedEvent?.Invoke();
		await GDTask.CompletedTask;
	}
}
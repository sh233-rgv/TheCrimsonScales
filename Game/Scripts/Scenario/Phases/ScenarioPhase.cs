using Fractural.Tasks;
using Godot;

public abstract class ScenarioPhase
{
	public virtual GDTask Activate()
	{
		Log.Write($"Started {GetType()}.");

		return GDTask.CompletedTask;
	}
}
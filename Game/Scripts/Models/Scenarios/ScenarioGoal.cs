public abstract class ScenarioGoal
{
	public abstract string Text { get; }
	public virtual int Order { get; }

	public ScenarioGoal(int order)
	{
		Order = order;
	}
}
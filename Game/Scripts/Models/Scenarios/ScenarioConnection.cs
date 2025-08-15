public class ScenarioConnection<T> : ScenarioConnection
	where T : ScenarioModel
{
	public ScenarioConnection(bool linked = false)
		: base(ModelDB.Scenario<T>(), linked)
	{
	}
}

public class ScenarioConnection
{
	public ScenarioModel To { get; }
	public bool Linked { get; }

	protected ScenarioConnection(ScenarioModel to, bool linked)
	{
		To = to;
		Linked = linked;
	}
}
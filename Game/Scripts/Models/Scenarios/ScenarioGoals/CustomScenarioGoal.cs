using System;
using Fractural.Tasks;

public class CustomScenarioGoal : ScenarioGoal
{
	private readonly Func<CustomScenarioGoal, GDTask> _onStart;

	public override string Text { get; }

	public CustomScenarioGoal(Func<CustomScenarioGoal, GDTask> onStart, string text, int order = 0)
		: base(order)
	{
		_onStart = onStart;
		Text = text;
	}

	public override async GDTask Start()
	{
		await base.Start();

		await _onStart.Invoke(this);
	}

	public new async GDTask Complete()
	{
		await base.Complete();
	}
}
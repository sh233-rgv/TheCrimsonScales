using System;

public class ScenarioRule
{
	public string Text { get; private set; }
	public int Order { get; }

	public bool Removed { get; private set; }

	public event Action<ScenarioRule> TextChangedEvent;
	public event Action<ScenarioRule> TextRemovedEvent;

	public ScenarioRule(string text, int order)
	{
		Text = text;
		Order = order;
	}

	public void SetText(string text)
	{
		Text = text;

		TextChangedEvent?.Invoke(this);
	}

	public void Remove()
	{
		Removed = true;

		TextRemovedEvent?.Invoke(this);
	}
}
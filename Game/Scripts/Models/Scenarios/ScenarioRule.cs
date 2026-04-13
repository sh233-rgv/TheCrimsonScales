using System;

public class ScenarioRule
{
	private TextHelper.LabelTextDelegate _getLabelText;

	public int Order { get; }

	public bool Removed { get; private set; }

	public event Action<ScenarioRule> TextChangedEvent;
	public event Action<ScenarioRule> TextRemovedEvent;

	public ScenarioRule(TextHelper.LabelTextDelegate getLabelText, int order)
	{
		_getLabelText = getLabelText;
		Order = order;
	}

	public string GetLabelText(RichTextParameters textParameters) => _getLabelText(textParameters);

	public void SetText(TextHelper.LabelTextDelegate getTextLabel)
	{
		_getLabelText = getTextLabel;

		TextChangedEvent?.Invoke(this);
	}

	public void Remove()
	{
		Removed = true;

		TextRemovedEvent?.Invoke(this);
	}
}
public abstract class ScenarioEffect
{
	public abstract string GetDescription(RichTextParameters richTextParameters);

	public abstract void Activate(Character character);
}
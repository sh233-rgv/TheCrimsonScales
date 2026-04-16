public class LabeledOptions<TValue> : LabeledOptions
{
	public LabeledOption<TValue>[] Options { get; }

	public override int OptionCount => Options.Length;

	public LabeledOptions(LabeledOption<TValue>[] options)
	{
		Options = options;
	}

	public override LabeledOption GetOption(int index)
	{
		return Options[index];
	}

	public override string GetLabel(SavedOption<int> savedOption)
	{
		return Options[savedOption.Value].Label;
	}

	public TValue GetValue(SavedOption<int> savedOption)
	{
		return Options[savedOption.Value].Value;
	}
}

public abstract class LabeledOptions
{
	public abstract int OptionCount { get; }
	public abstract LabeledOption GetOption(int index);
	public abstract string GetLabel(SavedOption<int> savedOption);
}
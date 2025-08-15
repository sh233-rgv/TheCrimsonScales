public abstract class OptionViewParameters<T> : OptionViewParameters
{
	public SavedOption<T> SavedOption { get; }

	public OptionViewParameters(string label, SavedOption<T> savedOption)
	{
		SavedOption = savedOption;
		Label = label;
	}
}

public abstract class OptionViewParameters
{
	public string Label { get; protected set; }

	public abstract string ScenePath { get; }
}
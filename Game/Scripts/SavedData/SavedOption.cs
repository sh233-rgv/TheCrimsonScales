using System;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SavedOption<T> : SavedOption
{
	[JsonProperty]
	public T Value { get; private set; }

	public event Action<T> ValueChangedEvent;

	public SavedOption(T value)
	{
		Value = value;
	}

	public void SetValue(T value)
	{
		if(Equals(value, Value))
		{
			return;
		}

		Value = value;
		ValueChangedEvent?.Invoke(Value);
	}
}

public abstract class SavedOption
{
}
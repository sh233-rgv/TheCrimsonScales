using System.Collections.Generic;
using Godot;

public class EventState(EventChoice choice)
{
	public EventChoice Choice { get; } = choice;

	private readonly Dictionary<string, object> _customValues = new Dictionary<string, object>();

	public void SetCustomValue(string key, object value)
	{
		_customValues[key] = value;
	}

	public T GetCustomValue<T>(string key)
	{
		if(!_customValues.TryGetValue(key, out object value))
		{
			//Log.Error($"Could not find custom value for: {source} with key: {key}");
			return default;
		}

		if(value is not T castValue)
		{
			Log.Error($"Could not cast custom value for key: {key}");
			return default;
		}

		return castValue;
	}

	public bool TryGetCustomValue<T>(string key, out T value)
	{
		if(!_customValues.TryGetValue(key, out object retrievedValue))
		{
			//Log.Error($"Could not find custom value for: {source} with key: {key}");
			value = default;
			return false;
		}

		if(retrievedValue is not T castValue)
		{
			Log.Error($"Could not cast custom value for key: {key}");
			value = default;
			return false;
		}

		value = castValue;
		return true;
	}
}
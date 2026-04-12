using System;
using System.Collections.Generic;
using System.Threading;
using Fractural.Tasks;
using Godot;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public abstract class Reward
{
	[JsonProperty]
	public bool Completed { get; private set; }

	[JsonProperty]
	public Dictionary<string, object> CustomValues { get; private set; } = new Dictionary<string, object>();

	public abstract RewardType Type { get; }

	public abstract string GetLabelText(RichTextParameters textParameters);

	public virtual async GDTask ImmediateResolve(SavedCampaign savedCampaign, CancellationToken cancellationToken)
	{
		await GDTask.CompletedTask;
	}

	public virtual void SubscribeDuringDowntime(SavedEventState savedEventState)
	{
	}

	public virtual void UnsubscribeDuringDowntime()
	{
	}

	public virtual async GDTask OnScenarioSetupPhaseCompleted()
	{
		await GDTask.CompletedTask;
	}

	protected void Complete()
	{
		Completed = true;

		UnsubscribeDuringDowntime();
	}

	protected void SetCustomValue(string key, object value)
	{
		CustomValues[key] = value;
	}

	protected T GetCustomValue<T>(string key)
	{
		if(!CustomValues.TryGetValue(key, out object value))
		{
			//Log.Error($"Could not find custom value for key: {key}");
			return default;
		}

		if(value is not T castValue)
		{
			Log.Error($"Could not cast custom value for key: {key}");
			return default;
		}

		return castValue;
	}

	protected bool TryGetCustomValue<T>(string key, out T value)
	{
		if(!CustomValues.TryGetValue(key, out object retrievedValue))
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
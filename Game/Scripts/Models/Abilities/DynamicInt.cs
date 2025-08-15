using Godot;

public class DynamicInt<TState>
{
	public int? Value { get; }
	public GetValueDelegate GetValueFunc { get; }

	public delegate int GetValueDelegate(TState state);

	public DynamicInt(int? value, GetValueDelegate getValueFunc)
	{
		Value = value;
		GetValueFunc = getValueFunc;
	}

	public DynamicInt(GetValueDelegate getValueFunc) : this(null, getValueFunc)
	{
	}

	public static implicit operator DynamicInt<TState>(int value) => new DynamicInt<TState>(value, null);
	public static implicit operator DynamicInt<TState>(GetValueDelegate getValueFunc) => new DynamicInt<TState>(null, getValueFunc);

	public int GetValue(TState state)
	{
		if(GetValueFunc != null)
		{
			return GetValueFunc(state) + (Value ?? 0);
		}

		if(!Value.HasValue)
		{
			Log.Error("Both Value and GetValue are null for this dynamic value.");
			return default;
		}

		return Value.Value;
	}
}
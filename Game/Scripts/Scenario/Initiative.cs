using System;

public readonly struct Initiative : IEquatable<Initiative>
{
	public bool Null { get; init; }
	public int MainInitiative { get; init; }
	public int SortingInitiative { get; init; }

	public override string ToString()
	{
		if(Null)
		{
			return "??";
		}

		return MainInitiative.ToString();
	}

	public bool Equals(Initiative other)
	{
		return Null == other.Null && MainInitiative == other.MainInitiative && SortingInitiative == other.SortingInitiative;
	}

	public override bool Equals(object obj)
	{
		return obj is Initiative other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Null, MainInitiative, SortingInitiative);
	}
}
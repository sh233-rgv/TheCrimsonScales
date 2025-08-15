using Newtonsoft.Json;
using System;

public struct ModelId : IEquatable<ModelId>, IComparable<ModelId>
{
	public static readonly ModelId None = new ModelId()
	{
		Category = "NONE",
		Entry = "NONE"
	};

	private readonly string _category;
	private readonly string _entry;

	public string Category
	{
		get => _category;
		init => _category = string.IsNullOrEmpty(_category) ? value : throw new InvalidOperationException("Category value has already been set.");
	}

	public string Entry
	{
		get => _entry;
		init => _entry = string.IsNullOrEmpty(_entry) ? value : throw new InvalidOperationException("Entry value has already been set.");
	}

	[JsonConstructor]
	public ModelId(string json)
		: this()
	{
		string[] strArray = json.Split('.');
		Category = strArray.Length == 2 ? strArray[0] : throw new JsonReaderException("'" + json + "' does not match the expected ContentId form.");
		Entry = strArray[1];
	}

	public override string ToString()
	{
		return Category + "." + Entry;
	}

	public static bool operator ==(ModelId first, ModelId second)
	{
		return Equals(first, second);
	}

	public static bool operator !=(ModelId first, ModelId second)
	{
		return !(first == second);
	}

	public bool Equals(ModelId other)
	{
		return Category == other.Category && Entry == other.Entry;
	}

	public override bool Equals(object obj)
	{
		return obj is ModelId other && Equals(other);
	}

	public override int GetHashCode()
	{
		return (_category, _entry).GetHashCode();
	}

	public int CompareTo(ModelId other)
	{
		int num = string.Compare(Category, other.Category, StringComparison.Ordinal);
		return num != 0 ? num : string.Compare(Entry, other.Entry, StringComparison.Ordinal);
	}
}
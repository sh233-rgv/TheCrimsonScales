using System;

[Flags]
public enum AOEHexType
{
	Red = 1,
	Gray = 2,
	Empty = 4,
	Yellow = 8,
	Marked = 16,
	Marked2 = 31,
}
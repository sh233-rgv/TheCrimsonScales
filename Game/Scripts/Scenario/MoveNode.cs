using System.Collections.Generic;

public class MoveNode
{
	public Hex Hex { get; }
	public int MoveSpent { get; }
	public int MoveLeft { get; }
	public int NegativeHexEncounteredCount { get; }
	public List<MoveNode> Parents { get; } = new List<MoveNode>();

	public MoveNode(Hex hex, int moveSpent, int moveLeft, int negativeHexEncounteredCount)
	{
		Hex = hex;
		MoveSpent = moveSpent;
		MoveLeft = moveLeft;
		NegativeHexEncounteredCount = negativeHexEncounteredCount;
	}

	public CompareResult CompareTo(MoveNode other)
	{
		if(other.NegativeHexEncounteredCount > NegativeHexEncounteredCount)
		{
			return CompareResult.Better;
		}

		if(NegativeHexEncounteredCount > other.NegativeHexEncounteredCount)
		{
			return CompareResult.Worse;
		}

		if(other.MoveSpent > MoveSpent)
		{
			return CompareResult.Better;
		}

		if(MoveSpent > other.MoveSpent)
		{
			return CompareResult.Worse;
		}

		return CompareResult.Equal;
	}
}
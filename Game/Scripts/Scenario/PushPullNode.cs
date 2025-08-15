using System.Collections.Generic;

public class PushPullNode
{
	public Hex Hex { get; }
	public int MoveSpent { get; }
	public int MoveLeft { get; }
	public List<PushPullNode> Parents { get; } = new List<PushPullNode>();

	public PushPullNode(Hex hex, int moveSpent, int moveLeft)
	{
		Hex = hex;
		MoveSpent = moveSpent;
		MoveLeft = moveLeft;
	}

	public CompareResult CompareTo(PushPullNode other)
	{
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
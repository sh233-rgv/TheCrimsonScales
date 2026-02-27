public class FocusNode
{
	public Figure Focus { get; }
	public int NegativeHexEncounteredCount { get; }
	public int MoveSpent { get; }
	public int RangeFromCurrentHex { get; }
	public int Initiative { get; }
	public MoveNode MoveNode { get; }

	public FocusNode(Figure focus, int negativeHexEncounteredCount, int moveSpent, int rangeFromCurrentHex, int initiative, MoveNode moveNode)
	{
		Focus = focus;
		NegativeHexEncounteredCount = negativeHexEncounteredCount;
		MoveSpent = moveSpent;
		RangeFromCurrentHex = rangeFromCurrentHex;
		Initiative = initiative;
		MoveNode = moveNode;
	}

	public CompareResult CompareTo(FocusNode other, ScenarioCheckEvents.FigureFocus.Parameters figureFocusParameters = null)
	{
		bool focusFarthest = figureFocusParameters?.FocusFarthest == true;
		if(NegativeHexEncounteredCount > other.NegativeHexEncounteredCount)
		{
			return CompareResult.Worse;
		}

		if(other.NegativeHexEncounteredCount > NegativeHexEncounteredCount)
		{
			return CompareResult.Better;
		}

		if(MoveSpent > other.MoveSpent)
		{
			return focusFarthest ? CompareResult.Better : CompareResult.Worse;
		}

		if(other.MoveSpent > MoveSpent)
		{
			return focusFarthest ? CompareResult.Worse : CompareResult.Better;
		}

		if(RangeFromCurrentHex > other.RangeFromCurrentHex)
		{
			return focusFarthest ? CompareResult.Better : CompareResult.Worse;
		}

		if(other.RangeFromCurrentHex > RangeFromCurrentHex)
		{
			return focusFarthest ? CompareResult.Worse : CompareResult.Better;
		}

		if(Initiative > other.Initiative)
		{
			return CompareResult.Worse;
		}

		if(other.Initiative > Initiative)
		{
			return CompareResult.Better;
		}

		return CompareResult.Equal;
	}
}
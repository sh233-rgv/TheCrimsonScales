public enum CardState
{
	PersistentLoss = -7,
	PersistentNoDeactivate = -6,
	Persistent = -5,
	RoundLoss = -4,
	Round = -3,
	//PlayingActive = -2,
	Playing = -1,
	Hand = 0,
	Discarded = 1,
	Lost = 2,
	UnrecoverablyLost = 3
}

public class CardStates
{
	public static bool IsLoss(CardState cardState)
	{
		return cardState is CardState.PersistentLoss or CardState.RoundLoss or CardState.Lost or CardState.UnrecoverablyLost;
	}

	public static bool IsRound(CardState cardState)
	{
		return cardState is CardState.RoundLoss or CardState.Round;
	}

	public static bool IsPersistent(CardState cardState)
	{
		return cardState is CardState.PersistentLoss or CardState.Persistent or CardState.PersistentNoDeactivate;
	}

	public static bool IsActive(CardState cardState)
	{
		return IsRound(cardState) || IsPersistent(cardState);
	}
}
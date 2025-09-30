using System.Collections.Generic;

public class CardSelectionListCategoryParameters
{
	public List<SavedAbilityCard> Cards { get; }
	public string HeaderLabel { get; }
	public string HeaderIconPath { get; }
	public CardState? CardState { get; }

	public bool HasHeader => !string.IsNullOrEmpty(HeaderLabel);

	public CardSelectionListCategoryParameters(List<SavedAbilityCard> cards, string headerLabel, string headerIconPath, CardState? cardState)
	{
		Cards = cards;
		HeaderLabel = headerLabel;
		HeaderIconPath = headerIconPath;
		CardState = cardState;
	}
}
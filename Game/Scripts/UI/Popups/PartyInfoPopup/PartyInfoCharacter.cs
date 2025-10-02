using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class PartyInfoCharacter : Control
{
	[Export]
	private TextureRect _portraitTextureRect;
	[Export]
	private Label _nameLabel;
	[Export]
	private CardSelectionList _cardSelectionList;

	public void Init(Character character)
	{
		_portraitTextureRect.SetTexture(character.PortraitTexture);
		_nameLabel.SetText(character.SavedCharacter.Name);

		List<CardSelectionListCategoryParameters> cardCategoryParameters = new List<CardSelectionListCategoryParameters>();

		List<AbilityCard> cards = character.Cards;
		if(cards != null && cards.Count > 0)
		{
			// Scenario setup completed, show all card states
			cardCategoryParameters.Add(CreateCategoryParameters(cards, OnCardPressed,
				[CardState.Playing], CardSelectionListCategoryType.Playing));
			cardCategoryParameters.Add(CreateCategoryParameters(cards, OnCardPressed,
				[CardState.Persistent, CardState.PersistentLoss, CardState.Round, CardState.RoundLoss], CardSelectionListCategoryType.Active));
			cardCategoryParameters.Add(CreateCategoryParameters(cards, OnCardPressed,
				[CardState.Hand], CardSelectionListCategoryType.Hand));
			cardCategoryParameters.Add(CreateCategoryParameters(cards, OnCardPressed,
				[CardState.Discarded], CardSelectionListCategoryType.Discarded));
			cardCategoryParameters.Add(CreateCategoryParameters(cards, OnCardPressed,
				[CardState.Lost], CardSelectionListCategoryType.Lost));
		}
		else
		{
			List<SavedAbilityCard> abilityCards = character.SavedCharacter.HandAbilityCardIndices
				.Select(handAbilityCardIndex => character.SavedCharacter.AvailableAbilityCards[handAbilityCardIndex]).ToList();

			cardCategoryParameters.Add(new CardSelectionListCategoryParameters(abilityCards, CardSelectionListCategoryType.None, null, null));
		}

		_cardSelectionList.Open(cardCategoryParameters, (cardA, cardB) => cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));
	}

	private CardSelectionListCategoryParameters CreateCategoryParameters(List<AbilityCard> cards,
		Action<CardSelectionCard> onCardPressed, //, Action<CardSelectionCard> onInitiativePressed,
		CardState[] cardStates, CardSelectionListCategoryType categoryType)
	{
		return new CardSelectionListCategoryParameters(
			cards.Where(card => cardStates.Contains(card.CardState)).Select(card => card.SavedAbilityCard).ToList(),
			categoryType, onCardPressed, null);
	}

	private void OnCardPressed(CardSelectionCard card)
	{
	}
}
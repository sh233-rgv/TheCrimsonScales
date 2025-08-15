using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CardSelectionPopup : CardSelectionPopupBase<CardSelectionPopup.Request>
{
	public class Request : CardSelectionPopupBaseRequest
	{
	}

	[Export]
	private Label _requiredCountLabel;

	protected override void OnOpen()
	{
		base.OnOpen();

		UpdateLabel();
	}

	protected override void OnClosed()
	{
		PopupRequest.SavedCharacter.HandAbilityCardIndices.Clear();
		foreach(CardSelectionPopupCard card in _leftCards)
		{
			SavedAbilityCard savedAbilityCard = PopupRequest.SavedCharacter.AvailableAbilityCards.Find(savedAbilityCard => savedAbilityCard.Model == card.AbilityCardModel);
			PopupRequest.SavedCharacter.HandAbilityCardIndices.Add(PopupRequest.SavedCharacter.AvailableAbilityCards.IndexOf(savedAbilityCard));
		}

		base.OnClosed();

		AppController.Instance.SaveFile.Save();
	}

	protected override void AddCards()
	{
		List<AbilityCardModel> handCards =
			PopupRequest.SavedCharacter.HandAbilityCardIndices.Select(cardIndex => PopupRequest.SavedCharacter.AvailableAbilityCards[cardIndex].Model).ToList();
		foreach(AbilityCardModel abilityCardModel in handCards)
		{
			AddCard(abilityCardModel, true, _leftCards);
		}

		List<AbilityCardModel> availableCards =
			PopupRequest.SavedCharacter.AvailableAbilityCards.Select(card => card.Model).Where(card => !handCards.Contains(card)).ToList();
		foreach(AbilityCardModel abilityCardModel in availableCards)
		{
			AddCard(abilityCardModel, false, _rightCards);
		}
	}

	protected override void SortList(List<CardSelectionPopupCard> cards, bool left)
	{
		//IList<AbilityCardModel> classAbilityCards = PopupRequest.SavedCharacter.ClassModel.AbilityCards;

		//cards.Sort((cardA, cardB) => classAbilityCards.IndexOf(cardA.AbilityCardModel).CompareTo(classAbilityCards.IndexOf(cardB.AbilityCardModel)));
		cards.Sort((cardA, cardB) => cardA.AbilityCardModel.Initiative.CompareTo(cardB.AbilityCardModel.Initiative));

		for(int i = 0; i < cards.Count; i++)
		{
			CardSelectionPopupCard card = cards[i];
			card.GetParent().MoveChild(card, i);
		}
	}

	private void UpdateLabel()
	{
		int currentHandSize = _leftCards.Count;
		int requiredHandSize = PopupRequest.SavedCharacter.ClassModel.HandSize;
		_requiredCountLabel.SetText($"{currentHandSize}/{requiredHandSize}");

		bool correctHandSize = currentHandSize == requiredHandSize;
		_requiredCountLabel.SetModulate(correctHandSize ? Colors.White : Color.FromHtml("ff5e3a"));

		SetCanClose(correctHandSize);
	}

	protected override void OnCardPressed(CardSelectionPopupCard card)
	{
		base.OnCardPressed(card);

		UpdateLabel();
	}
}
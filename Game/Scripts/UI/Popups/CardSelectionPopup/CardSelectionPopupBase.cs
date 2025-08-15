using System.Collections.Generic;
using Godot;

public class CardSelectionPopupBaseRequest : PopupRequest
{
	public SavedCharacter SavedCharacter { get; init; }
}

public abstract partial class CardSelectionPopupBase<T> : Popup<T> where T : CardSelectionPopupBaseRequest
{
	[Export]
	private PackedScene _cardScene;

	[Export]
	private Control _leftCardsParent;
	[Export]
	private Control _rightCardsParent;

	[Export]
	private CardSelectionPopupPreview _cardPreview;

	protected readonly List<CardSelectionPopupCard> _leftCards = new List<CardSelectionPopupCard>();
	protected readonly List<CardSelectionPopupCard> _rightCards = new List<CardSelectionPopupCard>();

	protected override void OnOpen()
	{
		base.OnOpen();

		AddCards();

		SortLists();
	}

	protected override void OnClosed()
	{
		base.OnClosed();

		foreach(CardSelectionPopupCard card in _leftCards)
		{
			card.QueueFree();
		}

		_leftCards.Clear();

		foreach(CardSelectionPopupCard card in _rightCards)
		{
			card.QueueFree();
		}

		_rightCards.Clear();
	}

	protected abstract void AddCards();

	protected void AddCard(AbilityCardModel abilityCardModel, bool left, List<CardSelectionPopupCard> cards, bool canSelect = true, bool grayscale = false)
	{
		Control parent = left ? _leftCardsParent : _rightCardsParent;

		CardSelectionPopupCard card = _cardScene.Instantiate<CardSelectionPopupCard>();
		parent.AddChild(card);
		card.Init(abilityCardModel, canSelect, grayscale);
		card.CardPressedEvent += OnCardPressed;
		card.InitiativePressedEvent += OnInitiativePressed;
		card.MouseEnteredEvent += OnMouseEntered;
		card.MouseExitedEvent += OnMouseExited;

		cards.Add(card);
	}

	private void SortLists()
	{
		SortList(_leftCards, true);
		SortList(_rightCards, false);
	}

	protected abstract void SortList(List<CardSelectionPopupCard> cards, bool left);

	protected virtual void OnCardPressed(CardSelectionPopupCard card)
	{
		if(_rightCards.Contains(card))
		{
			_rightCards.Remove(card);
			_leftCards.Add(card);
			card.GetParent().RemoveChild(card);
			_leftCardsParent.AddChild(card);
		}
		else
		{
			_leftCards.Remove(card);
			_rightCards.Add(card);
			card.GetParent().RemoveChild(card);
			_rightCardsParent.AddChild(card);
		}

		SortLists();
	}

	private void OnInitiativePressed(CardSelectionPopupCard card)
	{
	}

	private void OnMouseEntered(CardSelectionPopupCard card)
	{
		_cardPreview.Focus(card);
	}

	private void OnMouseExited(CardSelectionPopupCard card)
	{
		_cardPreview.Unfocus(card);
	}
}
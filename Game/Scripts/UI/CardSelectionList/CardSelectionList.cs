using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class CardSelectionList : Control
{
	[Export]
	private PackedScene _cardSelectionCardScene;
	[Export]
	private ScrollContainer _scrollContainer;
	[Export]
	private Control _cardParent;
	[Export]
	private Control _container;
	[Export]
	private CardSelectionCardPreview _cardPreview;

	private Comparison<SavedAbilityCard> _sortComparison;

	private Vector2 _cardSize;

	public List<CardSelectionCard> Cards { get; } = new List<CardSelectionCard>();

	private event Action<CardSelectionCard> CardPressedEvent;
	private event Action<CardSelectionCard> InitiativePressedEvent;

	public override void _Ready()
	{
		base._Ready();

		Close();
	}

	public void Open(IEnumerable<SavedAbilityCard> cards,
		Action<CardSelectionCard> cardPressed, Action<CardSelectionCard> initiativePressed,
		Comparison<SavedAbilityCard> sortComparison)
	{
		//bool wasOpen = false;
		if(Cards.Count > 0)
		{
			//wasOpen = true;

			foreach(CardSelectionCard item in Cards)
			{
				item.Reparent(_container);
				item.Destroy();
			}

			Cards.Clear();
		}

		_sortComparison = sortComparison;

		CardPressedEvent = cardPressed;
		InitiativePressedEvent = initiativePressed;

		List<SavedAbilityCard> sortedCards = cards.ToList();
		sortedCards.Sort(_sortComparison);

		int index = 0;
		foreach(SavedAbilityCard savedAbilityCard in sortedCards)
		{
			CardSelectionCard card = CreateCard(savedAbilityCard, GetPosition(index));
			Cards.Add(card);
			index++;
		}

		UpdateScrollRect();
	}

	public void Close()
	{
		foreach(CardSelectionCard card in Cards)
		{
			card.Reparent(_container);
			card.QueueFree();
		}

		Cards.Clear();

		_cardParent.CustomMinimumSize = new Vector2(_cardSize.X, 0f);
		_cardParent.Size = _cardParent.CustomMinimumSize;
		_scrollContainer.MouseFilter = MouseFilterEnum.Ignore;

		CardPressedEvent = null;
		InitiativePressedEvent = null;
	}

	public void AddCard(SavedAbilityCard savedAbilityCard)
	{
		List<SavedAbilityCard> sortedCards = Cards.Select(cardSelectionCard => cardSelectionCard.SavedAbilityCard).ToList();
		sortedCards.Add(savedAbilityCard);
		sortedCards.Sort(_sortComparison);

		int index = sortedCards.IndexOf(savedAbilityCard);
		CardSelectionCard card = CreateCard(savedAbilityCard, GetPosition(index));
		Cards.Insert(index, card);

		card.SetPivotOffset(card.Size * 0.5f);
		card.SetScale(Vector2.Zero);
		card.TweenScale(1f, 0.2f).SetEasing(Easing.OutBack).Play();

		ReorderCardsVisually();
		UpdateScrollRect();
	}

	public void RemoveCard(SavedAbilityCard savedAbilityCard)
	{
		for(int i = 0; i < Cards.Count; i++)
		{
			CardSelectionCard cardSelectionCard = Cards[i];
			if(cardSelectionCard.SavedAbilityCard == savedAbilityCard)
			{
				cardSelectionCard.QueueFree();
				Cards.RemoveAt(i);
				break;
			}
		}

		ReorderCardsVisually();
		UpdateScrollRect();
	}

	private CardSelectionCard CreateCard(SavedAbilityCard savedAbilityCard, float positionY)
	{
		CardSelectionCard card = _cardSelectionCardScene.Instantiate<CardSelectionCard>();
		_cardParent.AddChild(card);
		_cardSize = card.Size;
		card.Position = new Vector2(0f, positionY);
		card.Init(savedAbilityCard, true, InitiativePressedEvent != null);
		card.CardPressedEvent += OnCardPressed;
		card.InitiativePressedEvent += OnInitiativePressed;
		card.MouseEnteredEvent += OnMouseEntered;
		card.MouseExitedEvent += OnMouseExited;

		return card;
	}

	private float GetPosition(int index)
	{
		return index * _cardSize.Y;
	}

	private void ReorderCardsVisually()
	{
		for(int i = 0; i < Cards.Count; i++)
		{
			CardSelectionCard card = Cards[i];
			card.TweenPositionY(GetPosition(i), 0.3f).SetEasing(Easing.OutQuad).Play();
		}
	}

	private void UpdateScrollRect()
	{
		_scrollContainer.Size = new Vector2(_cardSize.X, _scrollContainer.Size.Y);
		_scrollContainer.Position = new Vector2(0f, _scrollContainer.Position.Y);
		_cardParent.CustomMinimumSize = new Vector2(_cardSize.X, Cards.Count * _cardSize.Y);
		_cardParent.Size = _cardParent.CustomMinimumSize;
		_scrollContainer.MouseFilter = _cardParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
		_cardParent.MouseFilter = _cardParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Pass : MouseFilterEnum.Stop;
	}

	private void OnCardPressed(CardSelectionCard card)
	{
		CardPressedEvent?.Invoke(card);
	}

	private void OnInitiativePressed(CardSelectionCard card)
	{
		InitiativePressedEvent?.Invoke(card);
	}

	private void OnMouseEntered(CardSelectionCard card)
	{
		_cardPreview?.Focus(card);
	}

	private void OnMouseExited(CardSelectionCard card)
	{
		_cardPreview?.Unfocus(card);
	}
}
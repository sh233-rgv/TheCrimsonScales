using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

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

	public List<CardSelectionCard> Cards { get; } = new List<CardSelectionCard>();

	private float _itemWidth;
	private bool _restingEnabled;

	private event Action<CardSelectionCard> CardPressedEvent;
	private event Action<CardSelectionCard> InitiativePressedEvent;

	public override void _Ready()
	{
		base._Ready();

		Close();
	}

	public void Open(IEnumerable<SavedAbilityCard> cards,
		Action<CardSelectionCard> cardPressed, Action<CardSelectionCard> initiativePressed)
	{
		bool wasOpen = false;
		if(Cards.Count > 0)
		{
			wasOpen = true;

			foreach(CardSelectionCard item in Cards)
			{
				item.Reparent(_container);
				item.Destroy();
			}

			Cards.Clear();
		}

		CardPressedEvent = cardPressed;
		InitiativePressedEvent = initiativePressed;

		List<SavedAbilityCard> sortedCards = cards.ToList();
		// sortedCards.Sort((cardA, cardB) =>
		// 	cardA.CardState.CompareTo(cardB.CardState) * 10 + cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));

		int index = 0;
		float cardSize = 0f;
		foreach(SavedAbilityCard savedAbilityCard in sortedCards)
		{
			CardSelectionCard card = _cardSelectionCardScene.Instantiate<CardSelectionCard>();
			_cardParent.AddChild(card);
			cardSize = card.Size.Y;
			card.Position = new Vector2(0f, index * card.Size.Y);
			card.Init(savedAbilityCard, true, InitiativePressedEvent != null);
			card.CardPressedEvent += OnCardPressed;
			card.InitiativePressedEvent += OnInitiativePressed;
			card.MouseEnteredEvent += OnMouseEntered;
			card.MouseExitedEvent += OnMouseExited;

			Cards.Add(card);
			index++;

			_itemWidth = card.Size.X;
		}

		_scrollContainer.Size = new Vector2(_itemWidth, _scrollContainer.Size.Y);
		_cardParent.CustomMinimumSize = new Vector2(_itemWidth, index * cardSize);
		_cardParent.Size = _cardParent.CustomMinimumSize;
		_scrollContainer.MouseFilter = _cardParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
		_cardParent.MouseFilter = _cardParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Pass : MouseFilterEnum.Stop;
	}

	public void Close()
	{
		foreach(CardSelectionCard card in Cards)
		{
			card.Reparent(_container);
			card.QueueFree();
			//item.Destroy();
		}

		Cards.Clear();

		_cardParent.CustomMinimumSize = new Vector2(_itemWidth, 0f);
		_cardParent.Size = _cardParent.CustomMinimumSize;
		_scrollContainer.MouseFilter = MouseFilterEnum.Ignore;

		CardPressedEvent = null;
		InitiativePressedEvent = null;
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
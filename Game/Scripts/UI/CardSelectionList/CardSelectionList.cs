using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CardSelectionList : Control
{
	[Export]
	private PackedScene _itemScene;
	[Export]
	private ScrollContainer _scrollContainer;
	[Export]
	private Control _cardParent;
	[Export]
	private Control _container;
	[Export]
	private CardSelectionCardPreview _cardPreview;
	[Export]
	private Control _longRestIndicatorContainer;

	public List<CardSelectionCard> Cards { get; } = new List<CardSelectionCard>();

	private float _itemWidth;
	private bool _restingEnabled;

	private event Action<CardSelectionCard> CardPressedEvent;
	private event Action<CardSelectionCard> InitiativePressedEvent;

	public override void _Ready()
	{
		base._Ready();

		_longRestIndicatorContainer.Scale = Vector2.Zero;

		Close();
	}

	public void Open(IEnumerable<AbilityCard> cards,
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

		List<AbilityCard> sortedCards = cards.ToList();
		sortedCards.Sort((cardA, cardB) =>
			cardA.CardState.CompareTo(cardB.CardState) * 10 + cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));

		int index = 0;
		float cardSize = 0f;
		foreach(AbilityCard abilityCard in sortedCards)
		{
			CardSelectionCard card = _itemScene.Instantiate<CardSelectionCard>();
			_cardParent.AddChild(card);
			cardSize = card.Size.Y;
			card.Position = new Vector2(0f, index * card.Size.Y);
			card.Init(abilityCard, index, wasOpen ? 0.3f : 0f, true, InitiativePressedEvent != null);
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
		foreach(CardSelectionCard item in Cards)
		{
			item.Reparent(_container);
			item.Destroy();
		}

		Cards.Clear();

		_cardParent.CustomMinimumSize = new Vector2(_itemWidth, 0f);
		_cardParent.Size = _cardParent.CustomMinimumSize;
		_scrollContainer.MouseFilter = MouseFilterEnum.Ignore;

		_longRestIndicatorContainer.Scale = Vector2.Zero;

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
		_cardPreview.Focus(card);
	}

	private void OnMouseExited(CardSelectionCard card)
	{
		_cardPreview.Unfocus(card);
	}
}
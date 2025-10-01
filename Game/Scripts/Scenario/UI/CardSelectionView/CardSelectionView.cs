using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class CardSelectionView : Control
{
	[Export]
	private Control _container;
	[Export]
	private CardSelectionList _cardSelectionList;
	[Export]
	private Control _restButtons;
	[Export]
	private BetterButton _shortRestButton;
	[Export]
	private BetterButton _longRestButton;
	[Export]
	private Control _longRestIndicatorContainer;

	private bool _longRestSelected;
	private bool _restingEnabled;

	public IEnumerable<CardSelectionCard> Cards => _cardSelectionList.Cards;

	private event Action ShortRestPressedEvent;
	private event Action LongRestPressedEvent;

	public override void _Ready()
	{
		base._Ready();

		_shortRestButton.Pressed += OnShortRestButtonPressed;
		_longRestButton.Pressed += OnLongRestButtonPressed;

		_shortRestButton.Scale = Vector2.Zero;
		_longRestButton.Scale = Vector2.Zero;
		_longRestIndicatorContainer.Scale = Vector2.Zero;

		Close();
	}

	public void Open(List<AbilityCard> cards,
		Action<CardSelectionCard> onCardPressed, Action<CardSelectionCard> onInitiativePressed,
		Action shortRestPressed, Action longRestPressed)
	{
		// bool wasOpen = false;
		// if(Cards.Count > 0)
		// {
		// 	wasOpen = true;
		//
		// 	for(int i = 0; i < Cards.Count; i++)
		// 	{
		// 		CardSelectionCard item = Cards[i];
		// 		item.Reparent(_container);
		// 		item.TweenOut(i * 0.03f);
		// 	}
		//
		// 	Cards.Clear();
		// }

		ShortRestPressedEvent = shortRestPressed;
		LongRestPressedEvent = longRestPressed;

		List<CardSelectionListCategoryParameters> cardCategoryParameters = new List<CardSelectionListCategoryParameters>();

		cardCategoryParameters.Add(CreateCategoryParameters(cards, onCardPressed, onInitiativePressed,
			[CardState.Playing], CardSelectionListCategoryType.Playing));
		cardCategoryParameters.Add(CreateCategoryParameters(cards, onCardPressed, onInitiativePressed,
			[CardState.Persistent, CardState.PersistentLoss, CardState.Round, CardState.RoundLoss], CardSelectionListCategoryType.Active));
		cardCategoryParameters.Add(CreateCategoryParameters(cards, onCardPressed, onInitiativePressed,
			[CardState.Hand], CardSelectionListCategoryType.Hand));
		cardCategoryParameters.Add(CreateCategoryParameters(cards, onCardPressed, onInitiativePressed,
			[CardState.Discarded], CardSelectionListCategoryType.Discarded));
		cardCategoryParameters.Add(CreateCategoryParameters(cards, onCardPressed, onInitiativePressed,
			[CardState.Lost], CardSelectionListCategoryType.Lost));

		_cardSelectionList.Open(cardCategoryParameters, (cardA, cardB) => cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));

		// List<AbilityCard> sortedCards = cards.ToList();
		// sortedCards.Sort((cardA, cardB) =>
		// 	cardA.CardState.CompareTo(cardB.CardState) * 10 + cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));
		//
		// int index = 0;
		// float cardSize = 0f;
		// foreach(AbilityCard abilityCard in sortedCards)
		// {
		// 	CardSelectionCard card = _itemScene.Instantiate<CardSelectionCard>();
		// 	_itemParent.AddChild(card);
		// 	cardSize = CardSelectionCard.Size.Y;
		// 	card.Position = new Vector2(0f, index * CardSelectionCard.Size.Y);
		// 	card.Init(abilityCard.SavedAbilityCard, true, InitiativePressedEvent != null);
		// 	card.TweenIn((wasOpen ? 0.3f : 0f) + index * 0.03f);
		// 	card.CardPressedEvent += OnCardPressed;
		// 	card.InitiativePressedEvent += OnInitiativePressed;
		// 	card.MouseEnteredEvent += OnMouseEntered;
		// 	card.MouseExitedEvent += OnMouseExited;
		//
		// 	Cards.Add(card);
		// 	index++;
		//
		// 	_itemWidth = CardSelectionCard.Size.X;
		// }
		//
		// _scrollContainer.Size = new Vector2(_itemWidth, _scrollContainer.Size.Y);
		// _itemParent.CustomMinimumSize = new Vector2(_itemWidth, index * cardSize);
		// _itemParent.Size = _itemParent.CustomMinimumSize;
		// _scrollContainer.MouseFilter = _itemParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
		// _itemParent.MouseFilter = _itemParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Pass : MouseFilterEnum.Stop;
	}

	public void Close()
	{
		// for(int i = 0; i < Cards.Count; i++)
		// {
		// 	CardSelectionCard item = Cards[i];
		// 	item.Reparent(_container);
		// 	item.TweenOut(i * 0.03f);
		// }
		//
		// Cards.Clear();

		_cardSelectionList.Close();

		_longRestIndicatorContainer.Scale = Vector2.Zero;
		_longRestButton.Position = new Vector2(0f, _longRestButton.Position.Y);

		SetRestingEnabled(false);
	}

	public void SetRestingEnabled(bool enabled)
	{
		if(_restingEnabled == enabled)
		{
			return;
		}

		_restingEnabled = enabled;

		if(_restingEnabled)
		{
			_shortRestButton.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack).Play();
			_longRestButton.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack).Play();
		}
		else
		{
			_shortRestButton.TweenScale(0f, 0.15f).SetEasing(Easing.InBack).Play();
			_longRestButton.TweenScale(0f, 0.15f).SetEasing(Easing.InBack).Play();
		}
	}

	public void SetLongRestSelected(bool selected)
	{
		if(_longRestSelected == selected)
		{
			return;
		}

		_longRestSelected = selected;

		if(_longRestSelected)
		{
			_longRestIndicatorContainer.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack).Play();
		}
		else
		{
			_longRestIndicatorContainer.TweenScale(0f, 0.15f).SetEasing(Easing.InBack).Play();
		}

		_longRestButton.TweenPositionX(_longRestSelected ? 30 : 0, 0.1f).SetEasing(Easing.OutBack).Play();
	}

	private CardSelectionListCategoryParameters CreateCategoryParameters(List<AbilityCard> cards,
		Action<CardSelectionCard> onCardPressed, Action<CardSelectionCard> onInitiativePressed,
		CardState[] cardStates, CardSelectionListCategoryType categoryType)
	{
		return new CardSelectionListCategoryParameters(
			cards.Where(card => cardStates.Contains(card.CardState)).Select(card => card.SavedAbilityCard).ToList(),
			categoryType, onCardPressed, onInitiativePressed);
	}

	private void OnShortRestButtonPressed()
	{
		ShortRestPressedEvent?.Invoke();
	}

	private void OnLongRestButtonPressed()
	{
		LongRestPressedEvent?.Invoke();
	}
}
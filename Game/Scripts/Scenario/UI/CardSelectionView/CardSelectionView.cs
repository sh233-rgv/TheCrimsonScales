using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class CardSelectionView : Control
{
	[Export]
	private PackedScene _itemScene;
	[Export]
	private ScrollContainer _scrollContainer;
	[Export]
	private Control _itemParent;
	[Export]
	private Control _container;
	[Export]
	private CardSelectionCardPreview _cardPreview;
	[Export]
	private Control _restButtons;
	[Export]
	private BetterButton _shortRestButton;
	[Export]
	private BetterButton _longRestButton;
	[Export]
	private Control _longRestIndicatorContainer;

	public List<CardSelectionCard> Cards { get; } = new List<CardSelectionCard>();

	private bool _longRestSelected;
	private float _itemWidth;
	private bool _restingEnabled;

	private event Action<CardSelectionCard> CardPressedEvent;
	private event Action<CardSelectionCard> InitiativePressedEvent;
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

	public void Open(IEnumerable<AbilityCard> cards,
		Action<CardSelectionCard> cardPressed, Action<CardSelectionCard> initiativePressed,
		Action shortRestPressed, Action longRestPressed)
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
		ShortRestPressedEvent = shortRestPressed;
		LongRestPressedEvent = longRestPressed;

		List<AbilityCard> sortedCards = cards.ToList();
		sortedCards.Sort((cardA, cardB) =>
			cardA.CardState.CompareTo(cardB.CardState) * 10 + cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));

		int index = 0;
		float cardSize = 0f;
		foreach(AbilityCard abilityCard in sortedCards)
		{
			CardSelectionCard card = _itemScene.Instantiate<CardSelectionCard>();
			_itemParent.AddChild(card);
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
		_itemParent.CustomMinimumSize = new Vector2(_itemWidth, index * cardSize);
		_itemParent.Size = _itemParent.CustomMinimumSize;
		_scrollContainer.MouseFilter = _itemParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
		_itemParent.MouseFilter = _itemParent.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Pass : MouseFilterEnum.Stop;
	}

	public void Close()
	{
		foreach(CardSelectionCard item in Cards)
		{
			item.Reparent(_container);
			item.Destroy();
		}

		Cards.Clear();

		//SetDeferred(PropertyName.Size, new Vector2(_itemWidth, _scrollContainer.Size.Y));
		//_scrollContainer.Size = new Vector2(_itemWidth, _scrollContainer.Size.Y);
		_itemParent.CustomMinimumSize = new Vector2(_itemWidth, 0f);
		_itemParent.Size = _itemParent.CustomMinimumSize;
		_scrollContainer.MouseFilter = MouseFilterEnum.Ignore;
		//_longRestButton.Position = new Vector2(0f, _longRestButton.Position.Y);

		_longRestIndicatorContainer.Scale = Vector2.Zero;
		_longRestButton.Position = new Vector2(0f, _longRestButton.Position.Y);

		SetRestingEnabled(false);

		CardPressedEvent = null;
		InitiativePressedEvent = null;
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

	private void OnShortRestButtonPressed()
	{
		ShortRestPressedEvent?.Invoke();
	}

	private void OnLongRestButtonPressed()
	{
		LongRestPressedEvent?.Invoke();
	}
}
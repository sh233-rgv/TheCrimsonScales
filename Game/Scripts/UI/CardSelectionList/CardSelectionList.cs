using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CardSelectionList : Control
{
	[Export]
	private PackedScene _cardSelectionListCategoryScene;

	[Export]
	private ScrollContainer _scrollContainer;
	[Export]
	private Control _categoriesContainer;
	[Export]
	private Control _container;
	[Export]
	private CardSelectionCardPreview _cardPreview;

	private readonly List<CardSelectionListCategory> _categories = new List<CardSelectionListCategory>();

	public IEnumerable<CardSelectionCard> Cards
	{
		get
		{
			foreach(CardSelectionListCategory category in _categories)
			{
				foreach(CardSelectionCard cardSelectionCard in category.Cards)
				{
					yield return cardSelectionCard;
				}
			}
		}
	}

	private event Action<CardSelectionCard> CardPressedEvent;
	private event Action<CardSelectionCard> InitiativePressedEvent;

	public override void _Ready()
	{
		base._Ready();

		Close();
	}

	public void Open(List<SavedAbilityCard> cards,
		Action<CardSelectionCard> cardPressed, Action<CardSelectionCard> initiativePressed,
		Comparison<SavedAbilityCard> sortComparison)
	{
		CardSelectionListCategoryParameters parameters = new CardSelectionListCategoryParameters(cards, null, null, null);
		Open([parameters], cardPressed, initiativePressed, sortComparison);
	}

	public void Open(List<CardSelectionListCategoryParameters> cardCategoryParameters,
		Action<CardSelectionCard> cardPressed, Action<CardSelectionCard> initiativePressed,
		Comparison<SavedAbilityCard> sortComparison)
	{
		foreach(CardSelectionListCategory category in _categories)
		{
			category.Destroy(_container);
		}

		_categories.Clear();

		foreach(CardSelectionListCategoryParameters cardSelectionListCategoryParameters in cardCategoryParameters)
		{
			CardSelectionListCategory category = _cardSelectionListCategoryScene.Instantiate<CardSelectionListCategory>();
			_categoriesContainer.AddChild(category);
			category.CardPressedEvent += OnCardPressed;
			category.InitiativePressedEvent += OnInitiativePressed;
			category.CardMouseEnteredEvent += OnMouseEntered;
			category.CardMouseExitedEvent += OnMouseExited;
			category.Init(cardSelectionListCategoryParameters, sortComparison);

			_categories.Add(category);
		}

		CardPressedEvent = cardPressed;
		InitiativePressedEvent = initiativePressed;

		UpdateScrollRect();
	}

	public void Close()
	{
		foreach(CardSelectionListCategory category in _categories)
		{
			category.Destroy(_container);
		}

		_categories.Clear();

		_categoriesContainer.CustomMinimumSize = new Vector2(_categoriesContainer.CustomMinimumSize.X, 0f);
		_categoriesContainer.Size = _categoriesContainer.CustomMinimumSize;
		_scrollContainer.MouseFilter = MouseFilterEnum.Ignore;

		CardPressedEvent = null;
		InitiativePressedEvent = null;
	}

	public void AddCard(SavedAbilityCard savedAbilityCard, CardState? cardState = null)
	{
		bool added = false;
		foreach(CardSelectionListCategory category in _categories)
		{
			if(category.Parameters.CardState == cardState)
			{
				category.AddCard(savedAbilityCard);
				added = true;
				break;
			}
		}

		if(!added)
		{
			Log.Error($"Could not add card {savedAbilityCard.Model.Name} with cardState {cardState} to any category.");
		}

		UpdateScrollRect();
	}

	public void RemoveCard(SavedAbilityCard savedAbilityCard)
	{
		bool removed = false;
		foreach(CardSelectionListCategory category in _categories)
		{
			foreach(CardSelectionCard cardSelectionCard in category.Cards)
			{
				if(cardSelectionCard.SavedAbilityCard == savedAbilityCard)
				{
					category.RemoveCard(savedAbilityCard);
					removed = true;
					break;
				}
			}
		}

		if(!removed)
		{
			Log.Error($"Could not remove card {savedAbilityCard.Model.Name} to any category.");
		}

		UpdateScrollRect();
	}

	private void UpdateScrollRect()
	{
		float totalSize = 0f;
		foreach(CardSelectionListCategory category in _categories)
		{
			category.SetPosition(new Vector2(0f, totalSize));
			category.UpdateVisuals();
			totalSize += category.Size.Y;
		}

		_scrollContainer.SetSize(new Vector2(CardSelectionCard.Size.X, _scrollContainer.Size.Y));
		_scrollContainer.SetPosition(new Vector2(0f, _scrollContainer.Position.Y));
		_categoriesContainer.SetCustomMinimumSize(new Vector2(_categoriesContainer.Size.X, totalSize));
		_categoriesContainer.SetSize(_categoriesContainer.CustomMinimumSize);
		_scrollContainer.SetMouseFilter(_categoriesContainer.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore);
		_categoriesContainer.SetMouseFilter(_categoriesContainer.Size.Y > _scrollContainer.Size.Y ? MouseFilterEnum.Pass : MouseFilterEnum.Stop);
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
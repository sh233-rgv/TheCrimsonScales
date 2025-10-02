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
	private Label _healthLabel;
	[Export]
	private Label _xpLabel;

	[Export]
	private CardSelectionList _cardSelectionList;

	[Export]
	private PartyInfoCharacterItem[] _baseItemSlots;
	[Export]
	private PackedScene _partyInfoCharacterItemScene;
	[Export]
	private Control _smallItemsParent;

	public void Init(Character character)
	{
		// Base info
		_portraitTextureRect.SetTexture(character.PortraitTexture);
		_nameLabel.SetText(character.SavedCharacter.Name);
		_healthLabel.SetText($"{character.Health}/{character.MaxHealth}");
		_xpLabel.SetText($"{character.ObtainedXP}");

		// Cards
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
			// Still selecting cards and items to bring into the scenario
			List<SavedAbilityCard> abilityCards = character.SavedCharacter.HandAbilityCardIndices
				.Select(handAbilityCardIndex => character.SavedCharacter.AvailableAbilityCards[handAbilityCardIndex]).ToList();

			cardCategoryParameters.Add(new CardSelectionListCategoryParameters(abilityCards, CardSelectionListCategoryType.None, null, null));
		}

		_cardSelectionList.Open(cardCategoryParameters, (cardA, cardB) => cardA.Model.Initiative.CompareTo(cardB.Model.Initiative));

		// Items
		List<ItemModel> equippedItems = character.Items.ToList();
		for(int i = 0; i < _baseItemSlots.Length; i++)
		{
			PartyInfoCharacterItem baseItemSlot = _baseItemSlots[i];
			string baseSlotItem = character.SavedCharacter.EquippedBaseSlotItems[i];
			ItemModel itemModel = ModelDB.GetById<ItemModel>(baseSlotItem);

			int equippedIndex = equippedItems.FindIndex(item => item.ImmutableInstance == item);

			if(equippedIndex >= 0)
			{
				ItemModel equippedItem = equippedItems[equippedIndex];
				equippedItems.RemoveAt(equippedIndex);
				baseItemSlot.Init((ItemType)i, equippedItem);
			}
			else
			{
				baseItemSlot.Init((ItemType)i, itemModel);
			}
		}

		int smallItemSlotCount = character.SavedCharacter.GetSmallItemSlotCount();

		for(int i = 0; i < smallItemSlotCount; i++)
		{
			string smallSlotItem = i < character.SavedCharacter.EquippedSmallItems.Count ? character.SavedCharacter.EquippedSmallItems[i] : null;
			ItemModel item = ModelDB.GetById<ItemModel>(smallSlotItem);

			PartyInfoCharacterItem equipmentSlot = _partyInfoCharacterItemScene.Instantiate<PartyInfoCharacterItem>();
			_smallItemsParent.AddChild(equipmentSlot);
			equipmentSlot.Init(ItemType.Small, item);
		}
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
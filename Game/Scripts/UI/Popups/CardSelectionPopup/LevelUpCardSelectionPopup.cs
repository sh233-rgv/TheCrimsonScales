using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class LevelUpCardSelectionPopup : CardSelectionPopupBase<LevelUpCardSelectionPopup.Request>
{
	public class Request : CardSelectionPopupBaseRequest
	{
	}

	protected override void OnOpen()
	{
		base.OnOpen();
	}

	protected override void OnClosed()
	{
		// PopupRequest.SavedCharacter.HandAbilityCardIndices.Clear();
		// foreach(CardSelectionPopupCard card in _leftCards)
		// {
		// 	SavedAbilityCard savedAbilityCard = PopupRequest.SavedCharacter.AvailableAbilityCards.Find(savedAbilityCard => savedAbilityCard.Model == card.AbilityCardModel);
		// 	PopupRequest.SavedCharacter.HandAbilityCardIndices.Add(PopupRequest.SavedCharacter.AvailableAbilityCards.IndexOf(savedAbilityCard));
		// }

		base.OnClosed();
	}

	protected override void AddCards()
	{
		if(!PopupRequest.SavedCharacter.LevelUpInProgress)
		{
			Log.Error("Opened the level up card selection popup while the character is not leveling up.");
			Close();
			return;
		}

		List<AbilityCardModel> availableCards =
			PopupRequest.SavedCharacter.AvailableAbilityCards.Select(savedCard => savedCard.Model).ToList();
		foreach(AbilityCardModel abilityCardModel in availableCards)
		{
			AddCard(abilityCardModel, true, _leftCards, false);
		}

		List<AbilityCardModel> unlockableCards =
			PopupRequest.SavedCharacter.ClassModel.AbilityCards.Where(card => !availableCards.Contains(card)).ToList();
		foreach(AbilityCardModel abilityCardModel in unlockableCards)
		{
			bool canAdd = abilityCardModel.Level <= PopupRequest.SavedCharacter.Level;
			AddCard(abilityCardModel, false, _rightCards, canAdd, !canAdd);
		}
	}

	protected override void SortList(List<CardSelectionPopupCard> cards, bool left)
	{
		if(left)
		{
			cards.Sort((cardA, cardB) => cardA.AbilityCardModel.Initiative.CompareTo(cardB.AbilityCardModel.Initiative));
		}
		else
		{
			cards.Sort((cardA, cardB) => GetUnlockableCardScore(cardA).CompareTo(GetUnlockableCardScore(cardB)));
		}

		for(int i = 0; i < cards.Count; i++)
		{
			CardSelectionPopupCard card = cards[i];
			card.GetParent().MoveChild(card, i);
		}

		int GetUnlockableCardScore(CardSelectionPopupCard card)
		{
			int score = 0;

			score += card.AbilityCardModel.Level * 1000;
			score += card.AbilityCardModel.Initiative * 1;

			return score;
		}
	}

	protected override void OnCardPressed(CardSelectionPopupCard card)
	{
		base.OnCardPressed(card);

		PopupRequest.SavedCharacter.AddLevelUpCard(card.AbilityCardModel);

		AppController.Instance.SaveFile.Save();

		Close();
	}
}
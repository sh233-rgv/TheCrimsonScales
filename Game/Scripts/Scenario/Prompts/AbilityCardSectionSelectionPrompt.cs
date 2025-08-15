using System;
using System.Collections.Generic;

public class AbilityCardSectionSelectionPrompt(List<CardPlayCardData> cardDatas, EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<AbilityCardSectionSelectionPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public int CardReferenceId { get; init; }
		public AbilityCardSection AbilityCardSection { get; init; }
	}

	protected override bool CanConfirm => false;
	protected override bool CanSkip => false;

	protected override void Enable()
	{
		base.Enable();

		GameController.Instance.CardPlayView.Open(cardDatas, OnCardSectionPressed);
	}

	protected override void Disable()
	{
		base.Disable();

		GameController.Instance.CardPlayView.Close();
	}

	private void OnCardSectionPressed(CardPlayCard card, AbilityCardSection section)
	{
		Complete(new Answer()
		{
			CardReferenceId = card.AbilityCard.ReferenceId,
			AbilityCardSection = section
		});
	}
}
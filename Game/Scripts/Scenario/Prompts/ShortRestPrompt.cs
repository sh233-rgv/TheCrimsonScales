using System;

public class ShortRestPrompt(Character character, bool canRedraw, EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<ShortRestPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
		public int AbilityCardReferenceId { get; init; }
		public bool Redraw { get; init; }
	}

	protected override void Enable()
	{
		base.Enable();

		GameController.Instance.ShortRestView.ConfirmedEvent += OnConfirmed;
		GameController.Instance.ShortRestView.RedrawEvent += OnRedraw;
		GameController.Instance.ShortRestView.Open(character, canRedraw);
		GameController.Instance.ChoiceButtonsView.Block(this);
	}

	protected override void Disable()
	{
		base.Disable();

		GameController.Instance.ShortRestView.ConfirmedEvent -= OnConfirmed;
		GameController.Instance.ShortRestView.RedrawEvent -= OnRedraw;
		GameController.Instance.ShortRestView.Close();
		GameController.Instance.ChoiceButtonsView.UnBlock(this);
	}

	protected override void ShowOptions()
	{
	}

	private void OnConfirmed(AbilityCard abilityCard)
	{
		Complete(new Answer()
		{
			AbilityCardReferenceId = abilityCard.ReferenceId,
			Redraw = false
		});
	}

	private void OnRedraw(AbilityCard abilityCard)
	{
		Complete(new Answer()
		{
			AbilityCardReferenceId = abilityCard.ReferenceId,
			Redraw = true
		});
	}
}
using System;

public class ConfirmPrompt(EffectCollection effectCollection, Func<string> getHintText, bool mandatory = false)
	: Prompt<ConfirmPrompt.Answer>(effectCollection, getHintText)
{
	protected override bool CanSkip => !mandatory;
	public class Answer : PromptAnswer
	{
		public bool Confirmed { get; init; }
	}

	protected override void Enable()
	{
		base.Enable();

		if(_authority is not Character)
		{
			Complete(true);
		}
	}

	protected override Answer CreateAnswer()
	{
		return new Answer()
		{
			Confirmed = true
		};
	}
}
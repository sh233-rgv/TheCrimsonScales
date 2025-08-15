using System;

public class ScenarioEventPrompt(EffectCollection effectCollection, Func<string> getHintText)
	: Prompt<ScenarioEventPrompt.Answer>(effectCollection, getHintText)
{
	public class Answer : PromptAnswer
	{
	}

	protected override bool CanSkip => false;

	protected override void Enable()
	{
		if(!EffectCollection.HasSelectableEffects)
		{
			Complete(true);
		}
		// else if(EffectCollection.ApplicableEffects.Count == 1 && EffectCollection.ApplicableEffects[0].EffectType == EffectType.SelectableMandatory)
		// {
		// 	Complete(new Answer()
		// 	{
		// 		SelectedEffectIndex = 0
		// 	});
		// }
		else
		{
			base.Enable();
		}
	}

	protected override Answer CreateAnswer()
	{
		return new Answer()
		{
		};
	}
}
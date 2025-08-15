using System;

public class SufferDamagePrompt(ScenarioEvents.SufferDamage.Parameters sufferDamageParameters, EffectCollection effectCollection, Func<string> getHintText)
	: ScenarioEventPrompt(effectCollection, getHintText)
{
	protected override void Enable()
	{
		base.Enable();

		if(EffectCollection.HasSelectableEffects)
		{
			GameController.Instance.SufferDamageView.Open(sufferDamageParameters.Figure, sufferDamageParameters.CalculatedCurrentDamage);
		}
	}

	protected override void Disable()
	{
		base.Disable();

		GameController.Instance.SufferDamageView.Close();
	}
}
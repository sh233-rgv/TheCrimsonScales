using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class EffectCollection
{
	public List<Effect> Effects { get; } = new List<Effect>();
	public List<Effect> ApplicableEffects { get; } = new List<Effect>();

	public bool HasSelectableEffects => ApplicableEffects.Any(effect => effect.EffectType is EffectType.Selectable or EffectType.SelectableMandatory);

	public EffectCollection(ScenarioEvent.ParametersBase parameters, IEnumerable<ScenarioEvent.Subscription> subscriptions)
	{
		foreach(ScenarioEvent.Subscription subscription in subscriptions)
		{
			Effect effect = new Effect(subscription, parameters, Effects.Count);
			effect.AppliedEvent += OnApplied;
			Effects.Add(effect);
		}

		Update();
	}

	public void Update()
	{
		ApplicableEffects.Clear();

		foreach(Effect effect in Effects)
		{
			effect.Update();

			if(effect.CanApply)
			{
				ApplicableEffects.Add(effect);
			}
		}
	}

	public async GDTask PerformBeforePrompt()
	{
		// First, perform all Visual effects
		for(int i = 0; i < ApplicableEffects.Count; i++)
		{
			Effect effect = ApplicableEffects[i];
			if(effect.EffectType == EffectType.Visuals)
			{
				await effect.Apply();

				// Reset the current index, allowing new visual effects to be applied
				i = -1;
			}
		}

		// Then, perform all MandatoryBeforeOptionals effects
		for(int i = 0; i < ApplicableEffects.Count; i++)
		{
			Effect effect = ApplicableEffects[i];
			if(effect.EffectType == EffectType.MandatoryBeforeOptionals)
			{
				await effect.Apply();

				// Reset the current index, allowing new applicable mandatory effects to be applied
				i = -1;
			}
		}
	}

	public async GDTask PerformAfterPrompt()
	{
		// Finally, perform all mandatory after optionals effects
		for(int i = 0; i < ApplicableEffects.Count; i++)
		{
			Effect effect = ApplicableEffects[i];
			if(effect.EffectType == EffectType.MandatoryAfterOptionals)
			{
				await effect.Apply();

				// Reset the current index, allowing new applicable mandatory effects to be applied
				i = -1;
			}
		}
	}

	private void OnApplied(Effect effect)
	{
		Update();
	}
}
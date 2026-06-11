using System.Collections.Generic;
using Godot;

public partial class InfoExtraEffectsView : Control
{
	[Export]
	private Control _effectParent;

	private readonly List<InfoExtraEffectBase> _extraEffects = new List<InfoExtraEffectBase>();

	public void Update(List<InfoExtraEffectParameters> parametersList)
	{
		foreach(InfoExtraEffectBase extraEffect in _extraEffects)
		{
			extraEffect.QueueFree();
		}

		_extraEffects.Clear();

		SetVisible(parametersList.Count > 0);

		foreach(InfoExtraEffectParameters parameters in parametersList)
		{
			InfoExtraEffectBase extraEffect = SceneLoader.InstantiateScene<InfoExtraEffectBase>(parameters.ScenePath);
			_effectParent.AddChild(extraEffect);
			extraEffect.Init(parameters);
			_extraEffects.Add(extraEffect);
		}
	}
}
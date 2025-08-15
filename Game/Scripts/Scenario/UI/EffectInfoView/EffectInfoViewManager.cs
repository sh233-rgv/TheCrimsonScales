using System.Collections.Generic;
using Godot;

public partial class EffectInfoViewManager : Control
{
	[Export]
	private Control _infoViewParent;

	private readonly Dictionary<Control, EffectInfoViewBase> _infoViews = new Dictionary<Control, EffectInfoViewBase>();

	public void CreateInfoView(Control parent, EffectInfoViewParameters parameters)
	{
		RemoveInfoView(parent);

		PackedScene buttonScene = ResourceLoader.Load<PackedScene>(parameters.ScenePath);
		EffectInfoViewBase infoView = buttonScene.Instantiate<EffectInfoViewBase>();
		_infoViewParent.AddChild(infoView);

		infoView.Init(parent, parameters);
		_infoViews.Add(parent, infoView);
	}

	public void RemoveInfoView(Control parent)
	{
		if(!_infoViews.TryGetValue(parent, out EffectInfoViewBase infoView))
		{
			return;
		}

		infoView.Destroy();

		_infoViews.Remove(parent);
	}
}
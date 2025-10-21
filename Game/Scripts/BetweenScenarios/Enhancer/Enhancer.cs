using Godot;
using GTweens.Builders;

public partial class Enhancer : BetweenScenariosAction
{
	[Export]
	private SubViewport _subViewport;

	[Export]
	private Node3D _3dRoot;

	private bool _animating;

	protected override bool SelectCharacter => true;
	protected override bool CustomTransitioning => _animating;

	protected override void AnimateIn(GTweenSequenceBuilder sequenceBuilder)
	{
		base.AnimateIn(sequenceBuilder);

		_3dRoot.SetVisible(true);

		_subViewport.SetUpdateMode(SubViewport.UpdateMode.WhenVisible);
	}

	protected override void AfterAnimateIn()
	{
		base.AfterAnimateIn();
	}

	protected override void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
		base.AnimateOut(sequenceBuilder);
	}

	protected override void AfterAnimateOut()
	{
		base.AfterAnimateOut();

		_3dRoot.SetVisible(true);

		_subViewport.SetUpdateMode(SubViewport.UpdateMode.Disabled);
	}
}
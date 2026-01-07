using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class Enhancer : BetweenScenariosAction
{
	[Export]
	private Node3D _3dRoot;
	[Export]
	private Node3D _crystalBall;

	protected override bool SelectCharacter => true;

	public override void _Ready()
	{
		base._Ready();

		_3dRoot.SetVisible(false);
	}

	protected override void AnimateIn(GTweenSequenceBuilder sequenceBuilder, BetweenScenariosAction previousActiveAction)
	{
		base.AnimateIn(sequenceBuilder, previousActiveAction);

		_3dRoot.SetVisible(true);
		_crystalBall.SetVisible(false);

		_crystalBall.SetPosition(new Vector3(0f, 5f, 0f));

		sequenceBuilder
			.AppendTime(0.2f)
			.AppendCallback((() =>
			{
				_crystalBall.SetVisible(true);
			}))
			.Append(_crystalBall.TweenPositionY(0f, 0.7f).SetEasing(Easing.InQuad))
			.Append(_crystalBall.TweenPositionY(0.1f, 0.12f))
			.Append(_crystalBall.TweenPositionY(0f, 0.08f))
			.AppendTime(1f);
	}

	protected override void AfterAnimateIn()
	{
		base.AfterAnimateIn();
	}

	protected override void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
		sequenceBuilder
			.Append(_crystalBall.TweenPositionY(5f, 0.5f))
			.AppendTime(0.8f);

		base.AnimateOut(sequenceBuilder);
	}

	protected override void AfterAnimateOut()
	{
		base.AfterAnimateOut();

		_3dRoot.SetVisible(false);
	}
}
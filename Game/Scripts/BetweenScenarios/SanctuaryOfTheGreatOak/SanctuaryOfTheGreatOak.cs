using Godot;
using GTweens.Builders;

public partial class SanctuaryOfTheGreatOak : BetweenScenariosAction
{
	[Export]
	private Node3D _3dRoot;

	[Export]
	private AnimationPlayer _animationPlayer;

	[Export]
	private StringName _moveInAnimationName;
	[Export]
	private StringName _moveOutAnimationName;

	protected override bool SelectCharacter => true;

	public override void _Ready()
	{
		base._Ready();

		_3dRoot.SetVisible(false);
	}

	protected override void AnimateIn(GTweenSequenceBuilder sequenceBuilder, BetweenScenariosAction previousActiveAction)
	{
		base.AnimateIn(sequenceBuilder, previousActiveAction);

		sequenceBuilder
			.AppendTime(previousActiveAction is ItemShop ? 0.5f : 0f)
			//.AppendTime(0.4f)
			.AppendCallback((() =>
			{
				this.DelayedCall(() =>
				{
					_3dRoot.SetVisible(true);
				});
				_animationPlayer.Play(_moveInAnimationName);
				// this.DelayedCall(() =>
				// {
				// 	_crystalBall.SetVisible(true);
				// });
			}))
			.AppendTime(1f);
	}

	protected override void AfterAnimateIn()
	{
		base.AfterAnimateIn();
	}

	protected override void AnimateOut(GTweenSequenceBuilder sequenceBuilder)
	{
		sequenceBuilder.AppendTime(1f).AppendCallback(() =>
		{
			_3dRoot.SetVisible(false);
			_animationPlayer.Play("RESET");
		});

		_animationPlayer.Play(_moveOutAnimationName);

		base.AnimateOut(sequenceBuilder);
	}

	protected override void AfterAnimateOut()
	{
		base.AfterAnimateOut();

		_3dRoot.SetVisible(false);
	}
}